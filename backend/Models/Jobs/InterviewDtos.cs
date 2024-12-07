namespace JobHuntBackend.Models.Jobs
{
  public class CreateInterviewRequest
  {
    public DateTime ScheduledAt { get; set; }
    public string InterviewType { get; set; } = string.Empty;
    public string? InterviewerName { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
  }

  public class UpdateInterviewRequest
  {
    public DateTime ScheduledAt { get; set; }
    public string InterviewType { get; set; } = string.Empty;
    public string? InterviewerName { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
    public bool Completed { get; set; }
  }

  public class InterviewResponse
  {
    public int Id { get; set; }
    public int JobId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string InterviewType { get; set; } = string.Empty;
    public string? InterviewerName { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
    public bool Completed { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}