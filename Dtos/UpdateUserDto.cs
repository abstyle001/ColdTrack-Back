namespace ColdTrack_Back.Dtos;

public class UpdateUserDto
{
    public string? NickName { get; set; }
    public string? City { get; set; }
    public string? Phone { get; set; }
    
    public IFormFile? File { get; set; }
}