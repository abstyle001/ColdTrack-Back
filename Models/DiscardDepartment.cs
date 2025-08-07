using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Models;

/*
 * 废弃的部门
 */
public class DiscardDepartment
{
    [Key]
    public long Id { get; set; }
    [Required]
    public string ParentId { get; set; }
    // 废弃的部门Id，与ParentId拼接可得到完整的DepartmentId
    [Required]
    public int ChildId { get; set; }
}