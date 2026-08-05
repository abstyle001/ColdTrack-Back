namespace ColdTrack_Back.Dtos;

/// <summary>
/// 用户简要信息，含部门与职位名称，供权限管理的穿梭面板使用。
/// </summary>
public class UserBriefDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NickName { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public List<string> DepartmentNames { get; set; } = new();
    public List<string> PositionNames { get; set; } = new();
}