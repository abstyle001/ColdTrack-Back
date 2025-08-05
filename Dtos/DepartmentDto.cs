namespace ColdTrack_Back.Dtos;

public class DepartmentDto
{
    public string Id { get; set; }
    // 部门名称
    public string Name { get; set; } = string.Empty;
    // 父节点Id
    public string ParentId { get; set; }
    // 职位层级
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
    public string CreatedAt { get; set; } = string.Empty;
}