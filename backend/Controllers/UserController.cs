using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobHuntBackend.Models.Users;
using JobHuntBackend.Services;
using System.Security.Claims;

namespace JobHuntBackend.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<UserProfileResponse>> GetProfile()
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var profile = await _userService.GetProfileAsync(userId!);
      return Ok(profile);
    }

    [HttpPut("profile")]
    public async Task<ActionResult<UserProfileResponse>> UpdateProfile(UpdateProfileRequest request)
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var profile = await _userService.UpdateProfileAsync(userId!, request);
      return Ok(profile);
    }

    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      await _userService.ChangePasswordAsync(userId!, request);
      return Ok(new { message = "Password updated successfully" });
    }

    [HttpGet("settings")]
    public async Task<ActionResult<UserSettings>> GetSettings()
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var settings = await _userService.GetSettingsAsync(userId!);
      return Ok(settings);
    }

    [HttpPut("settings")]
    public async Task<ActionResult<UserSettings>> UpdateSettings(UpdateSettingsRequest request)
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var settings = await _userService.UpdateSettingsAsync(userId!, request);
      return Ok(settings);
    }
  }
}