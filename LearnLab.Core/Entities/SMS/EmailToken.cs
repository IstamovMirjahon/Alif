namespace LearnLab.Core.Entities.SMS
{
    public class EmailToken
    {
        public EmailToken(string emailCode, string email)
        {
            EmailCode = emailCode;
            Email = email;
        }
        public string EmailCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
