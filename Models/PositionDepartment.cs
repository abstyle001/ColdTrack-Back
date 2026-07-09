using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Models;

/*
 * 职位部门关系表
 */
public class PositionDepartment
{
    [Key]
    public long Id { get; set; }
    [Required]
    public long PositionId { get; set; }
    [Required]
    public string DepartmentId { get; set; }
}