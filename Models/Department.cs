using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Models;

/*
 * 部门
 */
public class Department
{
    [Key]
    public long Id;
    // 部门名称
    [Required] public string Name { get; set; } = string.Empty;
    [MaxLength(50)]
    public string Code { get; set; }
}