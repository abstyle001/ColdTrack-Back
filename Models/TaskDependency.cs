using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColdTrack_Back.Models;

/*
 * 任务-任务依赖关联表（自引用多对多，复合主键 Successor + Predecessor）
 *
 * 方向：Successor（后置任务，被阻塞方）依赖 Predecessor（前置任务，阻塞方）。
 * 一条边携带一个依赖类型（FS/SS/FF/SF）。
 * 复合主键保证一对任务之间最多一条依赖边、一种类型。
 */
[Table("TaskDependencies")]
public class TaskDependency
{
    public enum DependencyTypeValue
    {
        FinishToStart = 0,   // FS 完成→开始：前置完成后，后置才能开始
        StartToStart = 1,    // SS 开始→开始：前置开始后，后置才能开始
        FinishToFinish = 2,  // FF 完成→完成：前置完成后，后置才能完成
        StartToFinish = 3,   // SF 开始→完成：前置开始后，后置才能完成
    }

    [Required]
    public long SuccessorTaskId { get; set; }
    [ForeignKey(nameof(SuccessorTaskId))]
    public TaskItem Successor { get; set; } = null!;

    [Required]
    public long PredecessorTaskId { get; set; }
    [ForeignKey(nameof(PredecessorTaskId))]
    public TaskItem Predecessor { get; set; } = null!;

    [Required]
    public DependencyTypeValue DependencyType { get; set; }
}
