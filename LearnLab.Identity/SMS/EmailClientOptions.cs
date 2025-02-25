namespace LearnLab.Core.Entities.Email
{
    public class EmailClientOptions
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public string SenderName { get; set; } = "UzWorks Support";
    }
}
