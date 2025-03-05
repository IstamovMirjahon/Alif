namespace LearnLab.Core.Entities.SMS
{
    public class EmailToken : BaseEntity
    {
        public EmailToken(string emailCode, string email)
        {
            EmailCode = emailCode;
            Email = email;
            CreateDate = UpdateDate = DateTimeOffset.UtcNow;
        }
        public string EmailCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
