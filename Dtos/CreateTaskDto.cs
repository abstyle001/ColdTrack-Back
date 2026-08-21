using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Dtos;

public class CreateTaskDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? AssigneeId { get; set; }

    public string? Priority { get; set; }

    public DateTime? Deadline { get; set; }

    public List<long>? TagIds { get; set; }
}
