namespace LearnLab.Identity.Email
{
    public interface IEmailSender
    {
        Task SendEmailOtpAsync(string email);
    }
}
