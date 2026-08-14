using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColdTrack_Back.Models;

[Table("TaskComments")]
public class TaskComment
{
    [Key]
    public long Id { get; set; }

    [Required]
    public long TaskId { get; set; }
    [ForeignKey(nameof(TaskId))]
    public TaskItem Task { get; set; } = null!;

    // 评论作者：用户被删除后保留评论（置空，展示为"已删除用户"）
    public string? AuthorId { get; set; }
    [ForeignKey(nameof(AuthorId))]
    public AppUser? Author { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
