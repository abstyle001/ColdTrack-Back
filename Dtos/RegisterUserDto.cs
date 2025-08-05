using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Dtos;

public class RegisterUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    public string? NickName { get; set; }
    public string? City { get; set; }
    public string? Phone { get; set; }
    public IFormFile? File { get; set; }
    // 所属职位
    public string? PositionId { get; set; }
    // 所属部门
    public string? DepartmentId { get; set; }
}

