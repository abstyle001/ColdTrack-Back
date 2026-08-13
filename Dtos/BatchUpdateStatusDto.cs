namespace ColdTrack_Back.Dtos;

public class BatchUpdateStatusDto
{
    public List<long> Ids { get; set; } = new();
    public string Status { get; set; } = string.Empty;
}
