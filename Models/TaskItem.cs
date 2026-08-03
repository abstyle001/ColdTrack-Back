using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColdTrack_Back.Models;

[Table("Tasks")]
public class TaskItem
{
    public enum StatusValue { Todo, InProgress, Review, Completed }
    public enum PriorityValue { Low, Medium, High, Urgent }

    [Key]
    public long Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? AssigneeId { get; set; }
    [ForeignKey(nameof(AssigneeId))]
    public AppUser? Assignee { get; set; }

    [Required]
    public string CreatorId { get; set; } = string.Empty;
    [ForeignKey(nameof(CreatorId))]
    public AppUser Creator { get; set; } = null!;

    [Required]
    public StatusValue Status { get; set; } = StatusValue.Todo;

    [Required]
    public PriorityValue Priority { get; set; } = PriorityValue.Medium;

    public DateTime? Deadline { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
