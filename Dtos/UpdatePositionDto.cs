namespace ColdTrack_Back.Dtos;

/*
 * 更新职位需要传入的信息
 */
public class UpdatePositionDto
{
    public string? Name { get; set; }
    public string? Duty { get; set; }
    public string? Workspace { get; set; }
    public string? Addition { get; set; }
}
