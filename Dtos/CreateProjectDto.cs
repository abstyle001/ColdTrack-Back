using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Dtos;

public class CreateProjectDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public string ManagerId { get; set; } = string.Empty;

    public string? Status { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public List<string>? MemberIds { get; set; }
}
