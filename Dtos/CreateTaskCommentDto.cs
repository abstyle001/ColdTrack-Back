using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Dtos;

public class CreateTaskCommentDto
{
    [Required(ErrorMessage = "评论内容不能为空")]
    public string? Content { get; set; }
}
