using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Models;

/*
 * 角色-权限关联表
 */
public class RolePermission
{
    [Required]
    public string RoleId { get; set; } = string.Empty;

    [Required]
    public long PermissionId { get; set; }
}
