namespace ColdTrack_Back.Dtos;

public class RoleDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<PermissionDto> Permissions { get; set; } = new();
}
