namespace ColdTrack_Back.Dtos;

public class TagDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
}
