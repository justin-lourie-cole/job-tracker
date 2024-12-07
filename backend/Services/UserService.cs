using Microsoft.EntityFrameworkCore;
using JobHuntBackend.Data;
using JobHuntBackend.Models.Users;
using JobHuntBackend.Models;

namespace JobHuntBackend.Services
{
  public interface IUserService
  {
    Task<UserProfileResponse> GetProfileAsync(string userId);
    Task<UserProfileResponse> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    Task ChangePasswordAsync(string userId, ChangePasswordRequest request);
    Task<UserSettings> GetSettingsAsync(string userId);
    Task<UserSettings> UpdateSettingsAsync(string userId, UpdateSettingsRequest request);
  }

  public class UserService : IUserService
  {
    private readonly JobHuntDbContext _context;

    public UserService(JobHuntDbContext context)
    {
      _context = context;
    }

    public async Task<UserProfileResponse> GetProfileAsync(string userId)
    {
      var user = await GetUserOrThrow(userId);
      return MapToProfileResponse(user);
    }

    public async Task<UserProfileResponse> UpdateProfileAsync(string userId, UpdateProfileRequest request)
    {
      var user = await GetUserOrThrow(userId);

      user.FirstName = request.FirstName;
      user.LastName = request.LastName;
      user.PhoneNumber = request.PhoneNumber;
      user.LinkedInUrl = request.LinkedInUrl;
      user.GithubUrl = request.GithubUrl;
      user.PortfolioUrl = request.PortfolioUrl;

      await _context.SaveChangesAsync();
      return MapToProfileResponse(user);
    }

    public async Task ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
      var user = await GetUserOrThrow(userId);

      if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
      {
        throw new InvalidOperationException("Current password is incorrect");
      }

      user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
      await _context.SaveChangesAsync();
    }

    public async Task<UserSettings> GetSettingsAsync(string userId)
    {
      var settings = await _context.UserSettings
          .FirstOrDefaultAsync(s => s.UserId == userId);

      return settings ?? new UserSettings();
    }

    public async Task<UserSettings> UpdateSettingsAsync(string userId, UpdateSettingsRequest request)
    {
      var settings = await _context.UserSettings
          .FirstOrDefaultAsync(s => s.UserId == userId);

      if (settings == null)
      {
        settings = new UserSettings
        {
          UserId = userId
        };
        _context.UserSettings.Add(settings);
      }

      settings.EmailNotifications = request.EmailNotifications;
      settings.WeeklyDigest = request.WeeklyDigest;
      settings.TimeZone = request.TimeZone;
      settings.DefaultCurrency = request.DefaultCurrency;

      await _context.SaveChangesAsync();
      return settings;
    }

    private async Task<User> GetUserOrThrow(string userId)
    {
      var user = await _context.Users.FindAsync(userId);
      if (user == null)
        throw new InvalidOperationException("User not found");
      return user;
    }

    private static UserProfileResponse MapToProfileResponse(User user)
    {
      return new UserProfileResponse
      {
        Id = user.Id,
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName,
        PhoneNumber = user.PhoneNumber,
        LinkedInUrl = user.LinkedInUrl,
        GithubUrl = user.GithubUrl,
        PortfolioUrl = user.PortfolioUrl,
        EmailVerified = user.EmailVerified
      };
    }
  }
}