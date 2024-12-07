namespace JobHuntBackend.Models.Users
{
  public class UserProfileResponse
  {
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string? PortfolioUrl { get; set; }
    public bool EmailVerified { get; set; }
  }

  public class UpdateProfileRequest
  {
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string? PortfolioUrl { get; set; }
  }

  public class ChangePasswordRequest
  {
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
  }

  public class UserSettings
  {
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public bool EmailNotifications { get; set; }
    public bool WeeklyDigest { get; set; }
    public string? TimeZone { get; set; }
    public string? DefaultCurrency { get; set; }
  }

  public class UpdateSettingsRequest
  {
    public bool EmailNotifications { get; set; }
    public bool WeeklyDigest { get; set; }
    public string? TimeZone { get; set; }
    public string? DefaultCurrency { get; set; }
  }
}