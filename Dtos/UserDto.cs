namespace ColdTrack_Back.Dtos;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NickName { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
    public string? Avatar { get; set; }
}