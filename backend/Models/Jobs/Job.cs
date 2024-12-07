namespace JobHuntBackend.Models.Jobs;

public class Job
{
  public int Id { get; set; }
  public string JobTitle { get; set; } = string.Empty;
  public string Company { get; set; } = string.Empty;
  public string Industry { get; set; } = string.Empty;
  public string CompanyOverview { get; set; } = string.Empty;
  public string Location { get; set; } = string.Empty;
  public string WhyIWantToWorkHere { get; set; } = string.Empty;
  public DateTime DateApplied { get; set; }
  public string JobPostingLink { get; set; } = string.Empty;
  public string ContactNameOrInfo { get; set; } = string.Empty;
  public string Notes { get; set; } = string.Empty;
  public string ResumeVersionUsed { get; set; } = string.Empty;
  public string Referral { get; set; } = string.Empty;
  public string SalaryRange { get; set; } = string.Empty;
  public string UserId { get; set; } = string.Empty;

  // New properties
  public JobStatus Status { get; set; } = JobStatus.Saved;
  public List<Interview> Interviews { get; set; } = new();
  public DateTime? NextInterviewDate { get; set; }
  public DateTime? LastFollowUpDate { get; set; }
  public DateTime? NextFollowUpDate { get; set; }
  public decimal? OfferedSalary { get; set; }
  public bool IsArchived { get; set; }
}