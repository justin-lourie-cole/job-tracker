using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using JobHuntBackend.Models.Auth;
using JobHuntBackend.Services;

namespace JobHuntBackend.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [EnableRateLimiting("AuthLimit")]  // Stricter rate limiting for auth endpoints
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
      _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
      try
      {
        var response = await _authService.RegisterAsync(request);
        return Ok(response);
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(new { error = ex.Message });
      }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
      try
      {
        var response = await _authService.LoginAsync(request);
        return Ok(response);
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(new { error = ex.Message });
      }
    }

    [HttpGet("validate-email")]
    public async Task<ActionResult<bool>> ValidateEmail([FromQuery] string email)
    {
      var isValid = await _authService.ValidateEmailAsync(email);
      return Ok(isValid);
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token)
    {
      try
      {
        await _authService.VerifyEmailAsync(token);
        return Ok(new { message = "Email verified successfully" });
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(new { error = ex.Message });
      }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
      try
      {
        await _authService.InitiatePasswordResetAsync(request.Email);
        return Ok(new { message = "Password reset email sent" });
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(new { error = ex.Message });
      }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
      try
      {
        await _authService.ResetPasswordAsync(request.Token, request.NewPassword);
        return Ok(new { message = "Password reset successfully" });
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(new { error = ex.Message });
      }
    }
  }
}