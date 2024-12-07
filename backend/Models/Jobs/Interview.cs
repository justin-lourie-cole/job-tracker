namespace JobHuntBackend.Models.Jobs
{
  public class Interview
  {
    public int Id { get; set; }
    public int JobId { get; set; }
    public Job Job { get; set; } = null!;
    public DateTime ScheduledAt { get; set; }
    public string InterviewType { get; set; } = string.Empty; // Phone, Video, OnSite, etc.
    public string? InterviewerName { get; set; }
    public string? Location { get; set; } // Could be link for virtual or address for in-person
    public string? Notes { get; set; }
    public bool Completed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  }
}