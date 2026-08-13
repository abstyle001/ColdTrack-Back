using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColdTrack_Back.Models;

/*
 * 权限目录：系统支持的全部权限点（资源:动作）
 */
public class Permission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    // 权限键，如 user.read / department.delete
    [Required]
    public string Key { get; set; } = string.Empty;

    // 显示名称，如 "用户查看"
    [Required]
    public string Name { get; set; } = string.Empty;

    // 分组，如 "用户管理" / "部门管理" / "系统设置"
    public string Group { get; set; } = string.Empty;

    // 说明
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
