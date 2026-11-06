namespace ColdTrack_Back.Dtos;

/*
 * 任务依赖展示 DTO。
 * TaskId = 关系中「另一方」的任务 Id：
 *   - 出现在 TaskDto.Dependencies 中时为前置任务 Id；
 *   - 出现在 TaskDto.Dependents 中时为后置任务 Id。
 */
public class TaskDependencyDto
{
    public long TaskId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string DependencyType { get; set; } = string.Empty;
}
