using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Models;

/*
 * 部门
 */
public class Department
{
    [Key]
    public string Id { get; set; }
    // 部门名称
    [Required] 
    public string Name { get; set; } = string.Empty;
    // 父节点Id
    [Required]
    public string ParentId { get; set; }
    // 职位层级
    [Required]
    public int Level { get; set; }
    // 部门说明
    public string Explain { get; set; }
    // 部门负责人的员工Id，指向人员（AppUser）实体类
    public string ManagerId { get; set; }
    // 部门工作地点
    public string Workspace { get; set; }
    // 附加信息
    public string? Addition { get; set; } = string.Empty;
    // 创建时间
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}