namespace LearnLab.Identity.SMS;

public class SmsClientOptions
{
    public const string SmsSectionName = "SMS";
    public string PhoneNumber { get; set; } = string.Empty;
    public string Message {  get; set; } = string.Empty;
    public string Sender {  get; set; } = string.Empty;
    public string UrlForLoginToEskiz { get; set; } = string.Empty;
    public string UrlForRefreshEskizToken {  get; set; } = string.Empty;
    public string UrlForSendSms { get; set; } = string.Empty;
    public string EskizEmail { get; set; } = string.Empty;
    public string EskizPassword { get; set; } = string.Empty;
}
