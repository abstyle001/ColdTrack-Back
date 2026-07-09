namespace ColdTrack_Back.Dtos;

/*
 * 用户职位聚合视图（含职位与部门信息）
 */
public class UserPositionViewDto
{
    public long PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public string? PositionDuty { get; set; }
    public string? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
}
