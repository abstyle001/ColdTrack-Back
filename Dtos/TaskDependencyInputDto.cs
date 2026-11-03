namespace ColdTrack_Back.Dtos;

/*
 * 任务依赖输入（创建/更新任务时随 DTO 传入）。
 * DependencyType 取值为 FinishToStart / StartToStart / FinishToFinish / StartToFinish。
 */
public class TaskDependencyInputDto
{
    public long PredecessorTaskId { get; set; }
    public string DependencyType { get; set; } = string.Empty;
}
