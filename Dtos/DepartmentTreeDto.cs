namespace ColdTrack_Back.Dtos;

/*
 * 部门树节点（含递归子节点）
 */
public class DepartmentTreeDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ParentId { get; set; } = string.Empty;
    public int Level { get; set; }
    public string Explain { get; set; } = string.Empty;
    public string ManagerId { get; set; } = string.Empty;
    public string? ManagerName { get; set; }
    public string Workspace { get; set; } = string.Empty;
    public string? Addition { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
    public List<DepartmentTreeDto> Children { get; set; } = [];
}
