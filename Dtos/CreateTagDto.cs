using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Dtos;

public class CreateTagDto
{
    [Required(ErrorMessage = "标签名称不能为空")]
    public string Name { get; set; } = string.Empty;

    public string? Color { get; set; }
}
