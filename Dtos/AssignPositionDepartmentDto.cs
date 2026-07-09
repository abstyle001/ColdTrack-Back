using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Dtos;

/*
 * 职位归属部门关联
 */
public class AssignPositionDepartmentDto
{
    [Required]
    public long PositionId { get; set; }
    [Required]
    public string DepartmentId { get; set; } = string.Empty;
}
