using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Dtos;

/*
 * 创建职位需要传入的信息
 */
public class CreatePositionDto
{
    // 职位名称
    [Required]
    public string Name { get; set; } = string.Empty;
    // 主要职责
    public string Duty { get; set; } = string.Empty;
    // 工作地点
    public string Workspace { get; set; } = string.Empty;
    // 附加信息
    public string? Addition { get; set; } = string.Empty;
}
