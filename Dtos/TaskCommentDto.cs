namespace ColdTrack_Back.Dtos;

public class TaskCommentDto
{
    public long Id { get; set; }
    public long TaskId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public string? AuthorAvatar { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
}
