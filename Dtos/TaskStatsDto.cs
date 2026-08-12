namespace ColdTrack_Back.Dtos;

public class TaskStatsDto
{
    public long Total { get; set; }
    public long TodoCount { get; set; }
    public long InProgressCount { get; set; }
    public long ReviewCount { get; set; }
    public long CompletedCount { get; set; }
    public long OverdueCount { get; set; }
    public long MyTaskCount { get; set; }
}
