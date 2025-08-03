using System.ComponentModel.DataAnnotations;

namespace ColdTrack_Back.Dtos;

public class FileDto
{
    [Required] public IFormFile? File { get; set; }
    public string Id { get; set; } = string.Empty;
}