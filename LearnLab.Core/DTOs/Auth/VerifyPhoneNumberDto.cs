using System.ComponentModel.DataAnnotations;

namespace LearnLab.Core.DTOs.Auth;

public class VerifyPhoneNumberDto
{
    [RegularExpression("^998\\d{9}$", ErrorMessage = "Invalid verification code.")]
    public string PhoneNumber { get; set; }
    public string Code { get; set; }

}
