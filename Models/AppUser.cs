using Microsoft.AspNetCore.Identity;

namespace ColdTrack_Back.Models;

public class AppUser : IdentityUser
{
    [PersonalData] public string? NickName { get; set; } = string.Empty;

    [PersonalData] public string? City { get; set; } = string.Empty;
    [PersonalData] public DateTime CreatedAt { get; set; }
    [PersonalData] public string? Avatar { get; set; }
}