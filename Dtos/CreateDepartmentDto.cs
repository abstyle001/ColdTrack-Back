using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Dtos;

/*
 * 创建部门需要传入的信息
 */
public class CreateDepartmentDto
{
      // 部门名称
      [Required]
      public string Name { get; set; } = string.Empty;
      // 父节点Id
      public string? ParentId { get; set; }
      // 部门说明
      public string Explain { get; set; } = string.Empty;
      // 部门负责人的员工Id，指向人员（AppUser）实体类
      public string? ManagerId { get; set; }
      // 部门工作地点
      public string Workspace { get; set; } = string.Empty;
      // 附加信息
      public string? Addition { get; set; } = string.Empty;
}