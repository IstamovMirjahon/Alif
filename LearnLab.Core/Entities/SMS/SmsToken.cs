using LearnLab.Core.Entities;

namespace LearnLab.Core.Entities.SMS;

public class SmsToken : BaseEntity
{
    public SmsToken(string smsCode, string phoneNumber)
    {
        SmsCode = smsCode;
        PhoneNumber = phoneNumber;
    }
    public string SmsCode { get; set; } =string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

}
 