using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Models;

/*
 * 职位
 */
public class Position
{
    [Key]
    public long Id { get; set; }
    // 职位名称
    [Required]
    public string Name { get; set; } = string.Empty;
    // 主要职责
    public string Duty { get; set; } = string.Empty;
    // 工作地点
    public string Workspace { get; set; } = string.Empty;
    // 附加信息
    public string? Addition { get; set; } = string.Empty;
    // 创建时间
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}