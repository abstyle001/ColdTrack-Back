using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Models;

/*
 * 用户职位关系表
 */
public class UserPosition
{
    [Key]
    public long Id { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public long PositionId { get; set; }
}