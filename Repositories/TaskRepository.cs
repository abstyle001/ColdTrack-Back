using ColdTrack_Back.Datas;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Models;
using Microsoft.EntityFrameworkCore;

namespace ColdTrack_Back.Repositories;

public class TaskRepository(ColdTrackDbContext db)
{
    public async Task<IEnumerable<TaskDto>> GetAll()
    {
        return await db.TaskItems
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => ToDto(t))
            .ToListAsync();
    }

    public async Task<TaskDto?> GetById(long id)
    {
        var task = await db.TaskItems
            .Include(t => t.Assignee)
            .Include(t => t.Creator)
            .FirstOrDefaultAsync(t => t.Id == id);
        return task == null ? null : ToDto(task);
    }

    public async Task<IEnumerable<TaskDto>> GetPage(int number, int size, string? status = null, string? priority = null, string? assigneeId = null)
    {
        var query = db.TaskItems.AsQueryable();

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<TaskItem.StatusValue>(status, out var s))
            query = query.Where(t => t.Status == s);
        if (!string.IsNullOrEmpty(priority) && Enum.TryParse<TaskItem.PriorityValue>(priority, out var p))
            query = query.Where(t => t.Priority == p);
        if (!string.IsNullOrEmpty(assigneeId))
            query = query.Where(t => t.AssigneeId == assigneeId);

        return await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((number - 1) * size)
            .Take(size)
            .Select(t => ToDto(t))
            .ToListAsync();
    }

    public int GetCount(string? status = null, string? priority = null, string? assigneeId = null)
    {
        var query = db.TaskItems.AsQueryable();
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<TaskItem.StatusValue>(status, out var s))
            query = query.Where(t => t.Status == s);
        if (!string.IsNullOrEmpty(priority) && Enum.TryParse<TaskItem.PriorityValue>(priority, out var p))
            query = query.Where(t => t.Priority == p);
        if (!string.IsNullOrEmpty(assigneeId))
            query = query.Where(t => t.AssigneeId == assigneeId);
        return query.Count();
    }

    public async Task<TaskDto> Create(CreateTaskDto dto, string creatorId)
    {
        var priority = TaskItem.PriorityValue.Medium;
        if (!string.IsNullOrEmpty(dto.Priority))
            Enum.TryParse<TaskItem.PriorityValue>(dto.Priority, out priority);

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            AssigneeId = dto.AssigneeId,
            CreatorId = creatorId,
            Priority = priority,
            Deadline = dto.Deadline,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await db.TaskItems.AddAsync(task);
        await db.SaveChangesAsync();
        return await GetById(task.Id) ?? throw new InvalidOperationException("Failed to load created task");
    }

    public async Task<TaskDto?> Update(long id, UpdateTaskDto dto)
    {
        var task = await db.TaskItems.FindAsync(id);
        if (task == null) return null;

        if (dto.Title != null) task.Title = dto.Title;
        if (dto.Description != null) task.Description = dto.Description;
        if (dto.AssigneeId != null) task.AssigneeId = dto.AssigneeId;
        if (dto.Status != null && Enum.TryParse<TaskItem.StatusValue>(dto.Status, out var s))
            task.Status = s;
        if (dto.Priority != null && Enum.TryParse<TaskItem.PriorityValue>(dto.Priority, out var p))
            task.Priority = p;
        if (dto.Deadline != null) task.Deadline = dto.Deadline;
        task.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return await GetById(task.Id);
    }

    public async Task<bool> Delete(long id)
    {
        var task = await db.TaskItems.FindAsync(id);
        if (task == null) return false;
        db.TaskItems.Remove(task);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteBatch(List<long> ids)
    {
        await db.TaskItems.Where(t => ids.Contains(t.Id)).ExecuteDeleteAsync();
        return true;
    }

    public async Task<TaskDto?> UpdateStatus(long id, string status)
    {
        var task = await db.TaskItems.FindAsync(id);
        if (task == null) return null;
        if (Enum.TryParse<TaskItem.StatusValue>(status, out var s))
        {
            task.Status = s;
            task.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }
        return await GetById(id);
    }

    private static TaskDto ToDto(TaskItem t) => new()
    {
        Id = t.Id,
        Title = t.Title,
        Description = t.Description,
        AssigneeId = t.AssigneeId,
        AssigneeName = t.Assignee != null ? t.Assignee.NickName : null,
        CreatorId = t.CreatorId,
        CreatorName = t.Creator != null ? (t.Creator.NickName ?? "") : "",
        Status = t.Status.ToString(),
        Priority = t.Priority.ToString(),
        Deadline = t.Deadline.HasValue ? t.Deadline.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
        CreatedAt = t.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
        UpdatedAt = t.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss")
    };
}