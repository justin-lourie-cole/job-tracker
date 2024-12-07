namespace JobHuntBackend.Models.Auth
{
  public class RegisterRequest
  {
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
  }

  public class LoginRequest
  {
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
  }

  public class AuthResponse
  {
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public UserDto User { get; set; } = new();
  }

  public class UserDto
  {
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
  }

  public class ForgotPasswordRequest
  {
    public string Email { get; set; } = string.Empty;
  }

  public class ResetPasswordRequest
  {
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
  }
}