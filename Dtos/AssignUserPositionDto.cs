using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Dtos;

/*
 * 用户分配职位关联
 */
public class AssignUserPositionDto
{
    [Required]
    public string UserId { get; set; } = string.Empty;
    [Required]
    public long PositionId { get; set; }
}
