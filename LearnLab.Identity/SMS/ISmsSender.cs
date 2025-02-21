namespace LearnLab.Identity.SMS
{
    public interface ISmsSender
    {
        Task SendSmsOtpAsync(string phoneNumber);
    }
}
