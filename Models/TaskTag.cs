using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColdTrack_Back.Models;

/*
 * 任务-标签关联表（复合主键）
 */
[Table("TaskTags")]
public class TaskTag
{
    [Required]
    public long TaskId { get; set; }
    [ForeignKey(nameof(TaskId))]
    public TaskItem Task { get; set; } = null!;

    [Required]
    public long TagId { get; set; }
    [ForeignKey(nameof(TagId))]
    public Tag Tag { get; set; } = null!;
}
