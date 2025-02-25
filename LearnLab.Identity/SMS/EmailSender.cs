using LearnLab.Core.Entities.Email;
using LearnLab.Core.Entities.SMS;
using LearnLab.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace LearnLab.Identity.Email;

public class EmailSender : IEmailSender
{
    private readonly EmailClientOptions _options;
    private readonly LearnLabIdentityDbContext _context;

    public EmailSender(IOptions<EmailClientOptions> options, LearnLabIdentityDbContext context)
    {
        _options = options.Value;
        _context = context;
    }

    public async Task SendEmailOtpAsync(string email)
    {
        var code = new Random().Next(1000, 9999).ToString();
        var emailToken = await _context.EmailTokens.FirstOrDefaultAsync(x => x.Email == email);

        if (emailToken != null)
            _context.EmailTokens.Remove(emailToken);

        await _context.EmailTokens.AddAsync(new EmailToken(code, email));
        await _context.SaveChangesAsync();

        var verificationMessage = $"Assalomu alaykum. UzWorks platformasi uchun tasdiqlash kodi: {code}";

        try
        {
            await SendEmailAsync(email, "Tasdiqlash Kodingiz", verificationMessage);
        }
        catch (Exception ex)
        {
            throw new LearnLabException("Failed to send email otp. Please try again later.", ex);
        }
    }

    private async Task SendEmailAsync(string recipientEmail, string subject, string message)
    {
        using var client = new SmtpClient(_options.SmtpServer, _options.SmtpPort)
        {
            Credentials = new NetworkCredential(_options.SmtpUsername, _options.SmtpPassword),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_options.SmtpUsername, _options.SenderName),
            Subject = subject,
            Body = message,
            IsBodyHtml = false
        };

        mailMessage.To.Add(recipientEmail);
        await client.SendMailAsync(mailMessage);
    }
}
