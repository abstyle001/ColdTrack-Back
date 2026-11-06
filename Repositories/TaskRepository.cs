using ColdTrack_Back.Datas;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Models;
using Microsoft.EntityFrameworkCore;

namespace ColdTrack_Back.Repositories;

public class TaskRepository(ColdTrackDbContext db)
{
    // 可见性：Admin 看全部；普通用户看「分配给自己的」「自己负责的项目下的」或「自己所在项目下的」任务
    private static IQueryable<TaskItem> ApplyVisibility(IQueryable<TaskItem> query, string userId, bool isAdmin)
        => isAdmin ? query : query.Where(t => t.AssigneeId == userId
            || t.Project.ManagerId == userId
            || t.Project.Members.Any(m => m.UserId == userId));

    public async Task<IEnumerable<TaskDto>> GetAll(string userId, bool isAdmin, long? projectId = null, string? assigneeId = null)
    {
        var query = db.TaskItems.AsQueryable();
        query = ApplyVisibility(query, userId, isAdmin);
        if (projectId.HasValue)
            query = query.Where(t => t.ProjectId == projectId.Value);
        if (!string.IsNullOrEmpty(assigneeId))
            query = query.Where(t => t.AssigneeId == assigneeId);
        return await query
            .OrderByDescending(t => t.CreatedAt)
            .Include(t => t.Project)
            .Include(t => t.TaskTags).ThenInclude(tt => tt.Tag)
            .Include(t => t.Dependencies).ThenInclude(d => d.Predecessor)
            .Include(t => t.Dependents).ThenInclude(d => d.Successor)
            .Select(t => ToDto(t))
            .ToListAsync();
    }

    public async Task<TaskDto?> GetById(long id)
    {
        var task = await db.TaskItems
            .Include(t => t.Assignee)
            .Include(t => t.Creator)
            .Include(t => t.Project)
            .Include(t => t.TaskTags).ThenInclude(tt => tt.Tag)
            .Include(t => t.Dependencies).ThenInclude(d => d.Predecessor)
            .Include(t => t.Dependents).ThenInclude(d => d.Successor)
            .FirstOrDefaultAsync(t => t.Id == id);
        return task == null ? null : ToDto(task);
    }

    public async Task<IEnumerable<TaskDto>> GetPage(int number, int size, string? status = null, string? priority = null, string? assigneeId = null, long? tagId = null, long? projectId = null, string userId = "", bool isAdmin = true)
    {
        var query = db.TaskItems.AsQueryable();
        query = ApplyVisibility(query, userId, isAdmin);

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<TaskItem.StatusValue>(status, out var s))
            query = query.Where(t => t.Status == s);
        if (!string.IsNullOrEmpty(priority) && Enum.TryParse<TaskItem.PriorityValue>(priority, out var p))
            query = query.Where(t => t.Priority == p);
        if (projectId.HasValue)
            query = query.Where(t => t.ProjectId == projectId.Value);
        if (!string.IsNullOrEmpty(assigneeId))
            query = query.Where(t => t.AssigneeId == assigneeId);
        if (tagId.HasValue)
            query = query.Where(t => t.TaskTags.Any(tt => tt.TagId == tagId.Value));

        return await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((number - 1) * size)
            .Take(size)
            .Include(t => t.Project)
            .Include(t => t.TaskTags).ThenInclude(tt => tt.Tag)
            .Include(t => t.Dependencies).ThenInclude(d => d.Predecessor)
            .Include(t => t.Dependents).ThenInclude(d => d.Successor)
            .Select(t => ToDto(t))
            .ToListAsync();
    }

    public int GetCount(string? status = null, string? priority = null, string? assigneeId = null, long? tagId = null, long? projectId = null, string userId = "", bool isAdmin = true)
    {
        var query = db.TaskItems.AsQueryable();
        query = ApplyVisibility(query, userId, isAdmin);
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<TaskItem.StatusValue>(status, out var s))
            query = query.Where(t => t.Status == s);
        if (!string.IsNullOrEmpty(priority) && Enum.TryParse<TaskItem.PriorityValue>(priority, out var p))
            query = query.Where(t => t.Priority == p);
        if (projectId.HasValue)
            query = query.Where(t => t.ProjectId == projectId.Value);
        if (!string.IsNullOrEmpty(assigneeId))
            query = query.Where(t => t.AssigneeId == assigneeId);
        if (tagId.HasValue)
            query = query.Where(t => t.TaskTags.Any(tt => tt.TagId == tagId.Value));
        return query.Count();
    }

    public async Task<TaskDto> Create(CreateTaskDto dto, string creatorId)
    {
        var projectExists = await db.Projects.AnyAsync(p => p.Id == dto.ProjectId);
        if (!projectExists)
            throw new InvalidOperationException("项目不存在");
        if (!string.IsNullOrEmpty(dto.AssigneeId))
        {
            var isMember = await db.ProjectMembers.AnyAsync(pm => pm.ProjectId == dto.ProjectId && pm.UserId == dto.AssigneeId);
            if (!isMember)
                throw new InvalidOperationException("负责人必须是项目成员");
        }

        var priority = TaskItem.PriorityValue.Medium;
        if (!string.IsNullOrEmpty(dto.Priority))
            Enum.TryParse<TaskItem.PriorityValue>(dto.Priority, out priority);

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            AssigneeId = dto.AssigneeId,
            CreatorId = creatorId,
            ProjectId = dto.ProjectId,
            Priority = priority,
            Deadline = dto.Deadline,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        await db.TaskItems.AddAsync(task);

        if (dto.TagIds != null && dto.TagIds.Count > 0)
        {
            var distinct = dto.TagIds.Distinct().ToList();
            var existing = await db.Tags.Where(t => distinct.Contains(t.Id)).Select(t => t.Id).ToListAsync();
            foreach (var tagId in existing)
                task.TaskTags.Add(new TaskTag { TagId = tagId });
        }

        if (dto.Dependencies != null && dto.Dependencies.Count > 0)
            await ApplyDependenciesAsync(task, dto.Dependencies);

        await db.SaveChangesAsync();
        return await GetById(task.Id) ?? throw new InvalidOperationException("Failed to load created task");
    }

    public async Task<TaskDto?> Update(long id, UpdateTaskDto dto)
    {
        var task = await db.TaskItems
            .Include(t => t.TaskTags)
            .Include(t => t.Dependencies).ThenInclude(d => d.Predecessor)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (task == null) return null;

        if (dto.Title != null) task.Title = dto.Title;
        if (dto.Description != null) task.Description = dto.Description;
        if (dto.AssigneeId != null) task.AssigneeId = dto.AssigneeId;
        if (dto.ProjectId.HasValue)
        {
            var projectExists = await db.Projects.AnyAsync(p => p.Id == dto.ProjectId.Value);
            if (!projectExists)
                throw new InvalidOperationException("项目不存在");
            // 任务挂到目标项目时，负责人（新设或已有的）必须是该项目成员
            var effectiveAssigneeId = dto.AssigneeId ?? task.AssigneeId;
            if (!string.IsNullOrEmpty(effectiveAssigneeId))
            {
                var isMember = await db.ProjectMembers.AnyAsync(pm => pm.ProjectId == dto.ProjectId.Value && pm.UserId == effectiveAssigneeId);
                if (!isMember)
                    throw new InvalidOperationException("负责人必须是项目成员");
            }
            task.ProjectId = dto.ProjectId.Value;
        }
        if (dto.Priority != null && Enum.TryParse<TaskItem.PriorityValue>(dto.Priority, out var p))
            task.Priority = p;
        if (dto.Deadline != null) task.Deadline = dto.Deadline;

        if (dto.TagIds != null)
        {
            var target = dto.TagIds.Distinct().ToList();
            var current = task.TaskTags.Select(tt => tt.TagId).ToList();

            var toRemove = current.Except(target).ToList();
            var toAdd = target.Except(current).ToList();
            var existingIds = await db.Tags.Where(t => toAdd.Contains(t.Id)).Select(t => t.Id).ToListAsync();

            foreach (var tagId in toRemove)
            {
                var row = task.TaskTags.First(tt => tt.TagId == tagId);
                task.TaskTags.Remove(row);
            }
            foreach (var tagId in existingIds)
                task.TaskTags.Add(new TaskTag { TaskId = id, TagId = tagId });
        }

        // 依赖整体替换（传入则重建；需在更改状态时先完成依赖校验）
        if (dto.Dependencies != null)
            await ApplyDependenciesAsync(task, dto.Dependencies);

        task.UpdatedAt = DateTime.Now;

        // 状态写入前做依赖校验（覆盖 PUT 整体更新这一改状态入口）
        if (dto.Status != null && Enum.TryParse<TaskItem.StatusValue>(dto.Status, out var targetStatus))
        {
            await ValidateStatusTransitionAsync(task, targetStatus);
            task.Status = targetStatus;
        }

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

    public async Task<bool> TaskExists(long id)
    {
        return await db.TaskItems.AnyAsync(t => t.Id == id);
    }

    public async Task<List<TaskCommentDto>> GetComments(long taskId)
    {
        var comments = await db.TaskComments
            .AsNoTracking()
            .Include(c => c.Author)
            .Where(c => c.TaskId == taskId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
        return comments.Select(ToCommentDto).ToList();
    }

    public async Task<TaskCommentDto?> AddComment(long taskId, string content, string authorId)
    {
        var taskExists = await db.TaskItems.AnyAsync(t => t.Id == taskId);
        if (!taskExists)
            return null;

        var comment = new TaskComment
        {
            TaskId = taskId,
            AuthorId = authorId,
            Content = content,
            CreatedAt = DateTime.Now
        };
        await db.TaskComments.AddAsync(comment);
        await db.SaveChangesAsync();

        var created = await db.TaskComments
            .Include(c => c.Author)
            .FirstOrDefaultAsync(c => c.Id == comment.Id);
        return created == null ? null : ToCommentDto(created);
    }

    public async Task<TaskDto?> UpdateStatus(long id, string status, string? assigneeId = null)
    {
        var task = await db.TaskItems
            .Include(t => t.Dependencies).ThenInclude(d => d.Predecessor)
            .FirstOrDefaultAsync(t => t.Id == id && (string.IsNullOrEmpty(assigneeId) || t.AssigneeId == assigneeId));
        if (task == null) return null;
        if (Enum.TryParse<TaskItem.StatusValue>(status, out var s))
        {
            await ValidateStatusTransitionAsync(task, s);
            task.Status = s;
            task.UpdatedAt = DateTime.Now;
            await db.SaveChangesAsync();
        }
        return await GetById(id);
    }

    public async Task<List<TaskDto>> UpdateStatusBatch(
        List<long> ids,
        string status,
        string? assigneeId = null)
    {
        if (ids == null || ids.Count == 0 ||
            !Enum.TryParse<TaskItem.StatusValue>(status, out var targetStatus))
            return new List<TaskDto>();

        var query = db.TaskItems.Where(t => ids.Contains(t.Id));
        if (!string.IsNullOrEmpty(assigneeId))
            query = query.Where(t => t.AssigneeId == assigneeId);

        var tasks = await query
            .Include(t => t.Project)
            .Include(t => t.TaskTags).ThenInclude(tt => tt.Tag)
            .Include(t => t.Dependencies).ThenInclude(d => d.Predecessor)
            .Include(t => t.Dependents).ThenInclude(d => d.Successor)
            .ToListAsync();

        // 保守校验：以批量执行前状态快照逐任务校验，任一阻塞则整批拒绝
        foreach (var task in tasks)
            await ValidateStatusTransitionAsync(task, targetStatus);

        foreach (var task in tasks)
        {
            task.Status = targetStatus;
            task.UpdatedAt = DateTime.Now;
        }
        await db.SaveChangesAsync();
        return tasks.Select(ToDto).ToList();
    }

    public async Task<TaskStatsDto> GetStats(string userId, bool isAdmin)
    {
        var now = DateTime.Now;
        var query = ApplyVisibility(db.TaskItems.AsQueryable(), userId, isAdmin);

        return new TaskStatsDto
        {
            Total = await query.CountAsync(),
            TodoCount = await query.CountAsync(t => t.Status == TaskItem.StatusValue.Todo),
            InProgressCount = await query.CountAsync(t => t.Status == TaskItem.StatusValue.InProgress),
            ReviewCount = await query.CountAsync(t => t.Status == TaskItem.StatusValue.Review),
            CompletedCount = await query.CountAsync(t => t.Status == TaskItem.StatusValue.Completed),
            OverdueCount = await query.CountAsync(t =>
                t.Status != TaskItem.StatusValue.Completed
                && t.Deadline.HasValue
                && t.Deadline.Value < now),
            MyTaskCount = await query.CountAsync(t =>
                t.AssigneeId == userId
                && t.Status != TaskItem.StatusValue.Completed),
        };
    }

    private static TaskCommentDto ToCommentDto(TaskComment c) => new()
    {
        Id = c.Id,
        TaskId = c.TaskId,
        Content = c.Content,
        AuthorId = c.AuthorId,
        AuthorName = c.Author != null ? (c.Author.NickName ?? c.Author.UserName ?? "") : null,
        AuthorAvatar = c.Author != null ? c.Author.Avatar : null,
        CreatedAt = c.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
    };

    private static TagDto ToTagDto(Tag t) => new()
    {
        Id = t.Id,
        Name = t.Name,
        Color = t.Color,
        CreatedAt = t.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
    };

    private static TaskDto ToDto(TaskItem t) => new()
    {
        Id = t.Id,
        Title = t.Title,
        Description = t.Description,
        AssigneeId = t.AssigneeId,
        AssigneeName = t.Assignee != null ? t.Assignee.NickName : null,
        CreatorId = t.CreatorId,
        CreatorName = t.Creator != null ? (t.Creator.NickName ?? "") : "",
        ProjectId = t.ProjectId,
        ProjectName = t.Project != null ? t.Project.Name : "",
        Status = t.Status.ToString(),
        Priority = t.Priority.ToString(),
        Deadline = t.Deadline.HasValue ? t.Deadline.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
        CreatedAt = t.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
        UpdatedAt = t.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
        Tags = t.TaskTags.Select(tt => ToTagDto(tt.Tag)).ToList(),
        Dependencies = t.Dependencies
            .Where(d => d.Predecessor != null)
            .Select(d => new TaskDependencyDto
            {
                TaskId = d.PredecessorTaskId,
                Title = d.Predecessor.Title,
                DependencyType = d.DependencyType.ToString()
            }).ToList(),
        Dependents = t.Dependents
            .Where(d => d.Successor != null)
            .Select(d => new TaskDependencyDto
            {
                TaskId = d.SuccessorTaskId,
                Title = d.Successor.Title,
                DependencyType = d.DependencyType.ToString()
            }).ToList()
    };

    // ===== 任务依赖：写入与校验 =====

    /// <summary>
    /// 将输入依赖解析并整体替换到 task.Dependencies。
    /// 校验：依赖类型合法、去重、前置任务存在、同项目、非自依赖；合并后做环路检测。
    /// </summary>
    private async Task ApplyDependenciesAsync(TaskItem task, List<TaskDependencyInputDto> inputs)
    {
        var parsed = new List<(long PredecessorId, TaskDependency.DependencyTypeValue Type)>();
        foreach (var input in inputs)
        {
            if (!Enum.TryParse<TaskDependency.DependencyTypeValue>(input.DependencyType, out var type))
                throw new InvalidOperationException("无效的依赖类型");
            parsed.Add((input.PredecessorTaskId, type));
        }

        // 去重（按前置任务 Id，同一前置只保留一条）
        var distinct = parsed
            .GroupBy(p => p.PredecessorId)
            .Select(g => g.First())
            .ToList();

        if (distinct.Count == 0)
        {
            task.Dependencies.Clear();
            return;
        }

        var predecessorIds = distinct.Select(p => p.PredecessorId).ToList();
        var predecessors = await db.TaskItems
            .Where(t => predecessorIds.Contains(t.Id))
            .Select(t => new { t.Id, t.ProjectId, t.Title })
            .ToDictionaryAsync(t => t.Id);

        foreach (var p in distinct)
        {
            if (p.PredecessorId == task.Id)
                throw new InvalidOperationException("任务不能依赖自身");
            if (!predecessors.TryGetValue(p.PredecessorId, out var pred))
                throw new InvalidOperationException("前置任务不存在");
            if (pred.ProjectId != task.ProjectId)
                throw new InvalidOperationException("前置任务必须与后置任务属于同一项目");
        }

        // 环路检测（防止整体替换引入环）
        var newEdges = distinct.Select(p => p.PredecessorId).ToHashSet();
        await DetectCycleAsync(task.Id, newEdges);

        // 校验通过后整体替换
        task.Dependencies.Clear();
        foreach (var p in distinct)
        {
            task.Dependencies.Add(new TaskDependency
            {
                SuccessorTaskId = task.Id,
                PredecessorTaskId = p.PredecessorId,
                DependencyType = p.Type
            });
        }
    }

    /// <summary>
    /// 环路检测：从 successor 出发沿 Predecessor 边 DFS，若可达自身则成环。
    /// 加载全量依赖边构建邻接表，保证不遗漏中间节点，正确性优先（任务规模有限，可接受）。
    /// </summary>
    private async Task DetectCycleAsync(long successorId, HashSet<long> proposedPredecessors)
    {
        // 加载全量依赖边，构建「后置 → 前置」邻接表
        var edges = new Dictionary<long, List<long>>();
        var rows = await db.TaskDependencies
            .AsNoTracking()
            .Select(d => new { d.SuccessorTaskId, d.PredecessorTaskId })
            .ToListAsync();
        foreach (var r in rows)
        {
            if (!edges.TryGetValue(r.SuccessorTaskId, out var list))
                edges[r.SuccessorTaskId] = list = new List<long>();
            list.Add(r.PredecessorTaskId);
        }

        // 用提议集合整体替换 successor 的出边
        edges[successorId] = proposedPredecessors.ToList();

        // 迭代式 DFS：successor 能否沿依赖边回到自身
        var stack = new Stack<long>();
        var visited = new HashSet<long>();
        stack.Push(successorId);
        while (stack.Count > 0)
        {
            var cur = stack.Pop();
            if (edges.TryGetValue(cur, out var nexts))
            {
                foreach (var next in nexts)
                {
                    if (next == successorId)
                        throw new InvalidOperationException("存在循环依赖");
                    if (visited.Add(next))
                        stack.Push(next);
                }
            }
        }
    }

    /// <summary>
    /// 依赖校验：将任务置为 newStatus 时检查其前置依赖是否满足约束（懒校验）。
    /// 按 PredecessorTaskId 直接查询前置任务状态，不依赖导航属性是否已加载，
    /// 以便在「同一次更新中新增依赖并改状态」时也能正确校验。
    /// </summary>
    private async Task ValidateStatusTransitionAsync(TaskItem task, TaskItem.StatusValue newStatus)
    {
        if (newStatus == TaskItem.StatusValue.Todo)
            return; // 回到待办/未开始，无约束

        var predecessorIds = task.Dependencies
            .Select(d => d.PredecessorTaskId)
            .Distinct()
            .ToList();
        if (predecessorIds.Count == 0)
            return;

        var preds = await db.TaskItems
            .Where(t => predecessorIds.Contains(t.Id))
            .Select(t => new { t.Id, t.Status, t.Title })
            .ToDictionaryAsync(t => t.Id);

        foreach (var dep in task.Dependencies)
        {
            if (!preds.TryGetValue(dep.PredecessorTaskId, out var pred))
                continue; // 前置任务已不存在（罕见），忽略

            switch (dep.DependencyType)
            {
                case TaskDependency.DependencyTypeValue.FinishToStart:
                    if (pred.Status != TaskItem.StatusValue.Completed)
                        throw new InvalidOperationException($"前置任务「{pred.Title}」尚未完成，无法开始");
                    break;
                case TaskDependency.DependencyTypeValue.StartToStart:
                    if (pred.Status == TaskItem.StatusValue.Todo)
                        throw new InvalidOperationException($"前置任务「{pred.Title}」尚未开始");
                    break;
                case TaskDependency.DependencyTypeValue.FinishToFinish:
                    if (newStatus == TaskItem.StatusValue.Completed
                        && pred.Status != TaskItem.StatusValue.Completed)
                        throw new InvalidOperationException($"前置任务「{pred.Title}」尚未完成，无法完成");
                    break;
                case TaskDependency.DependencyTypeValue.StartToFinish:
                    if (newStatus == TaskItem.StatusValue.Completed
                        && pred.Status == TaskItem.StatusValue.Todo)
                        throw new InvalidOperationException($"前置任务「{pred.Title}」尚未开始，无法完成");
                    break;
            }
        }
    }
}
