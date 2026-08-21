namespace ColdTrack_Back.Dtos;

public class UpdateTaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? AssigneeId { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public DateTime? Deadline { get; set; }
    public List<long>? TagIds { get; set; }
}
