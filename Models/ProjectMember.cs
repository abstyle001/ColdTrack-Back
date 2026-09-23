using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColdTrack_Back.Models;

/*
 * 项目-成员关联表（复合主键）
 */
[Table("ProjectMembers")]
public class ProjectMember
{
    [Required]
    public long ProjectId { get; set; }
    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;

    [Required]
    public string UserId { get; set; } = string.Empty;
    [ForeignKey(nameof(UserId))]
    public AppUser User { get; set; } = null!;
}
