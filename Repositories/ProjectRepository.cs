using ColdTrack_Back.Datas;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Models;
using Microsoft.EntityFrameworkCore;

namespace ColdTrack_Back.Repositories;

public class ProjectRepository(ColdTrackDbContext db)
{
    public async Task<IEnumerable<ProjectDto>> GetAll(string? status = null, string? managerId = null, string? keyword = null)
    {
        var query = db.Projects.AsQueryable();
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<Project.StatusValue>(status, out var s))
            query = query.Where(p => p.Status == s);
        if (!string.IsNullOrEmpty(managerId))
            query = query.Where(p => p.ManagerId == managerId);
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(p => p.Name.Contains(keyword));
        var projects = await query
            .OrderByDescending(p => p.CreatedAt)
            .Include(p => p.Manager)
            .Include(p => p.Members).ThenInclude(m => m.User)
            .ToListAsync();
        var projectIds = projects.Select(p => p.Id).ToList();
        var taskCounts = await db.TaskItems
            .Where(t => projectIds.Contains(t.ProjectId))
            .GroupBy(t => t.ProjectId)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Key, x => x.Count);
        return projects.Select(p => ToDto(p, taskCounts.GetValueOrDefault(p.Id))).ToList();
    }

    public async Task<ProjectDto?> GetById(long id)
    {
        var project = await db.Projects
            .Include(p => p.Manager)
            .Include(p => p.Members).ThenInclude(m => m.User)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (project == null) return null;
        var taskCount = await db.TaskItems.CountAsync(t => t.ProjectId == id);
        return ToDto(project, taskCount);
    }

    public async Task<ProjectDto> Create(CreateProjectDto dto)
    {
        var managerExists = await db.Users.AnyAsync(u => u.Id == dto.ManagerId);
        if (!managerExists)
            throw new InvalidOperationException("负责人不存在");

        var status = Project.StatusValue.InProgress;
        if (!string.IsNullOrEmpty(dto.Status))
            Enum.TryParse<Project.StatusValue>(dto.Status, out status);

        var project = new Project
        {
            Name = dto.Name,
            Description = dto.Description,
            ManagerId = dto.ManagerId,
            Status = status,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await db.Projects.AddAsync(project);

        var memberIds = dto.MemberIds?.Distinct().ToList() ?? new List<string>();
        // 负责人自动加入成员
        if (!memberIds.Contains(dto.ManagerId))
            memberIds.Add(dto.ManagerId);
        var existing = await db.Users.Where(u => memberIds.Contains(u.Id)).Select(u => u.Id).ToListAsync();
        foreach (var userId in existing)
            project.Members.Add(new ProjectMember { UserId = userId });

        await db.SaveChangesAsync();
        return await GetById(project.Id) ?? throw new InvalidOperationException("Failed to load created project");
    }

    public async Task<ProjectDto?> Update(long id, UpdateProjectDto dto)
    {
        var project = await db.Projects
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (project == null) return null;

        if (dto.Name != null) project.Name = dto.Name;
        if (dto.Description != null) project.Description = dto.Description;
        if (dto.ManagerId != null)
        {
            var managerExists = await db.Users.AnyAsync(u => u.Id == dto.ManagerId);
            if (!managerExists)
                throw new InvalidOperationException("负责人不存在");
            project.ManagerId = dto.ManagerId;
        }
        if (dto.Status != null && Enum.TryParse<Project.StatusValue>(dto.Status, out var s))
            project.Status = s;
        if (dto.StartDate != null) project.StartDate = dto.StartDate;
        if (dto.EndDate != null) project.EndDate = dto.EndDate;

        if (dto.MemberIds != null)
        {
            var target = dto.MemberIds.Distinct().ToList();
            var current = project.Members.Select(m => m.UserId).ToList();

            var toRemove = current.Except(target).ToList();
            var toAdd = target.Except(current).ToList();
            var existingIds = await db.Users.Where(u => toAdd.Contains(u.Id)).Select(u => u.Id).ToListAsync();

            foreach (var userId in toRemove)
            {
                var row = project.Members.First(m => m.UserId == userId);
                project.Members.Remove(row);
            }
            foreach (var userId in existingIds)
                project.Members.Add(new ProjectMember { ProjectId = id, UserId = userId });
        }

        // 更换负责人或调整成员后，保证负责人始终在成员中
        if (project.ManagerId != null && project.Members.All(m => m.UserId != project.ManagerId))
            project.Members.Add(new ProjectMember { ProjectId = id, UserId = project.ManagerId });

        project.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return await GetById(project.Id);
    }

    public async Task<(bool ok, string? error)> Delete(long id)
    {
        var project = await db.Projects.FindAsync(id);
        if (project == null) return (false, "项目不存在");
        var hasTasks = await db.TaskItems.AnyAsync(t => t.ProjectId == id);
        if (hasTasks) return (false, "项目下还有任务，无法删除");
        // 显式清理成员关联，数据库级 Cascade 作为兜底
        await db.ProjectMembers.Where(pm => pm.ProjectId == id).ExecuteDeleteAsync();
        db.Projects.Remove(project);
        await db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<bool> IsManager(long projectId, string userId)
        => await db.Projects.AnyAsync(p => p.Id == projectId && p.ManagerId == userId);

    public async Task<bool> IsManagerOfTask(long taskId, string userId)
        => await db.TaskItems.AnyAsync(t => t.Id == taskId && t.Project.ManagerId == userId);

    private static ProjectDto ToDto(Project p, int taskCount) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        ManagerId = p.ManagerId,
        ManagerName = p.Manager != null ? (p.Manager.NickName ?? p.Manager.UserName) : null,
        Status = p.Status.ToString(),
        StartDate = p.StartDate.HasValue ? p.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
        EndDate = p.EndDate.HasValue ? p.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
        CreatedAt = p.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
        UpdatedAt = p.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
        MemberCount = p.Members.Count,
        TaskCount = taskCount,
        Members = p.Members.Select(m => new UserBriefDto
        {
            Id = m.UserId,
            UserName = m.User.UserName ?? "",
            Email = m.User.Email ?? "",
            NickName = m.User.NickName ?? "",
            Avatar = m.User.Avatar
        }).ToList()
    };
}
