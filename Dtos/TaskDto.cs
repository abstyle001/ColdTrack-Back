namespace ColdTrack_Back.Dtos;

public class TaskDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? AssigneeId { get; set; }
    public string? AssigneeName { get; set; }
    public string CreatorId { get; set; } = string.Empty;
    public string CreatorName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string? Deadline { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
    public string UpdatedAt { get; set; } = string.Empty;
    public List<TagDto> Tags { get; set; } = new();
}
