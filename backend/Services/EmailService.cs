using System.Net.Mail;
using Microsoft.Extensions.Options;
using JobHuntBackend.Models;

namespace JobHuntBackend.Services
{
  public interface IEmailService
  {
    Task SendEmailAsync(string to, string subject, string htmlContent);
    Task SendVerificationEmailAsync(string to, string verificationToken);
    Task SendPasswordResetEmailAsync(string to, string resetToken);
  }

  public class EmailService : IEmailService
  {
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
      _settings = settings.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlContent)
    {
      var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
      {
        EnableSsl = true,
        Credentials = new System.Net.NetworkCredential(_settings.SmtpUsername, _settings.SmtpPassword)
      };

      var mailMessage = new MailMessage
      {
        From = new MailAddress(_settings.FromEmail, _settings.FromName),
        Subject = subject,
        Body = htmlContent,
        IsBodyHtml = true
      };
      mailMessage.To.Add(to);

      await client.SendMailAsync(mailMessage);
    }

    public async Task SendVerificationEmailAsync(string to, string verificationToken)
    {
      var verificationLink = $"{_settings.WebsiteBaseUrl}/verify-email?token={verificationToken}";
      var htmlContent = $@"
                <h2>Verify your email</h2>
                <p>Click the link below to verify your email address:</p>
                <p><a href='{verificationLink}'>Verify Email</a></p>
            ";

      await SendEmailAsync(to, "Verify your email", htmlContent);
    }

    public async Task SendPasswordResetEmailAsync(string to, string resetToken)
    {
      var resetLink = $"{_settings.WebsiteBaseUrl}/reset-password?token={resetToken}";
      var htmlContent = $@"
                <h2>Reset your password</h2>
                <p>Click the link below to reset your password:</p>
                <p><a href='{resetLink}'>Reset Password</a></p>
            ";

      await SendEmailAsync(to, "Reset your password", htmlContent);
    }
  }
}