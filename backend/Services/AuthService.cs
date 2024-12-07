using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using JobHuntBackend.Data;
using JobHuntBackend.Models;
using JobHuntBackend.Models.Auth;

namespace JobHuntBackend.Services
{
  public interface IAuthService
  {
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<bool> ValidateEmailAsync(string email);
    Task VerifyEmailAsync(string token);
    Task InitiatePasswordResetAsync(string email);
    Task ResetPasswordAsync(string token, string newPassword);
  }

  public class AuthService : IAuthService
  {
    private readonly JobHuntDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;

    public AuthService(JobHuntDbContext context, ITokenService tokenService, IEmailService emailService)
    {
      _context = context;
      _tokenService = tokenService;
      _emailService = emailService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
      if (await _context.Users.AnyAsync(u => u.Email == request.Email))
      {
        throw new InvalidOperationException("Email already registered");
      }

      var user = new User
      {
        Email = request.Email,
        FirstName = request.FirstName,
        LastName = request.LastName,
        PasswordHash = HashPassword(request.Password),
        VerificationToken = GenerateRandomToken(),
        VerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
      };

      _context.Users.Add(user);
      await _context.SaveChangesAsync();

      // Generate tokens
      var tokens = _tokenService.GenerateTokens(user.Id);

      // Store refresh token
      var refreshToken = new RefreshToken
      {
        Token = tokens.RefreshToken,
        UserId = user.Id,
        ExpiryDate = DateTime.UtcNow.AddDays(7),
        CreatedDate = DateTime.UtcNow
      };
      _context.RefreshTokens.Add(refreshToken);
      await _context.SaveChangesAsync();

      // TODO: Send verification email

      return new AuthResponse
      {
        AccessToken = tokens.AccessToken,
        RefreshToken = tokens.RefreshToken,
        User = MapToDto(user)
      };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
      var user = await _context.Users
          .FirstOrDefaultAsync(u => u.Email == request.Email);

      if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
      {
        throw new InvalidOperationException("Invalid credentials");
      }

      var tokens = _tokenService.GenerateTokens(user.Id);

      var refreshToken = new RefreshToken
      {
        Token = tokens.RefreshToken,
        UserId = user.Id,
        ExpiryDate = DateTime.UtcNow.AddDays(7),
        CreatedDate = DateTime.UtcNow
      };
      _context.RefreshTokens.Add(refreshToken);
      await _context.SaveChangesAsync();

      return new AuthResponse
      {
        AccessToken = tokens.AccessToken,
        RefreshToken = tokens.RefreshToken,
        User = MapToDto(user)
      };
    }

    public async Task<bool> ValidateEmailAsync(string email)
    {
      return !await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task VerifyEmailAsync(string token)
    {
      var user = await _context.Users
          .FirstOrDefaultAsync(u => u.VerificationToken == token);

      if (user == null)
        throw new InvalidOperationException("Invalid verification token");

      if (user.VerificationTokenExpiry < DateTime.UtcNow)
        throw new InvalidOperationException("Verification token has expired");

      user.EmailVerified = true;
      user.VerificationToken = null;
      user.VerificationTokenExpiry = null;

      await _context.SaveChangesAsync();
    }

    public async Task InitiatePasswordResetAsync(string email)
    {
      var user = await _context.Users
          .FirstOrDefaultAsync(u => u.Email == email);

      if (user == null)
        throw new InvalidOperationException("User not found");

      user.ResetPasswordToken = GenerateRandomToken();
      user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddHours(1);
      await _context.SaveChangesAsync();

      await _emailService.SendPasswordResetEmailAsync(email, user.ResetPasswordToken);
    }

    public async Task ResetPasswordAsync(string token, string newPassword)
    {
      var user = await _context.Users
          .FirstOrDefaultAsync(u => u.ResetPasswordToken == token);

      if (user == null)
        throw new InvalidOperationException("Invalid reset token");

      if (user.ResetPasswordTokenExpiry < DateTime.UtcNow)
        throw new InvalidOperationException("Reset token has expired");

      user.PasswordHash = HashPassword(newPassword);
      user.ResetPasswordToken = null;
      user.ResetPasswordTokenExpiry = null;

      await _context.SaveChangesAsync();
    }

    private static string HashPassword(string password)
    {
      return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static bool VerifyPassword(string password, string hash)
    {
      return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    private static string GenerateRandomToken()
    {
      var randomBytes = new byte[32];
      using var rng = RandomNumberGenerator.Create();
      rng.GetBytes(randomBytes);
      return Convert.ToBase64String(randomBytes);
    }

    private static UserDto MapToDto(User user)
    {
      return new UserDto
      {
        Id = user.Id,
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName,
        EmailVerified = user.EmailVerified
      };
    }
  }
}