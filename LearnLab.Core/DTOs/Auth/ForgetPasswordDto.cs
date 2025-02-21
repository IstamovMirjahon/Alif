using System.ComponentModel.DataAnnotations;
namespace LearnLab.Core.DTOs.Auth;

public class ForgetPasswordDto
{
    [Required(ErrorMessage = "Phone number is reuqired.")]
    [RegularExpression("^998\\d{9}$", ErrorMessage = "Invalid phone number.")]
    public string PhoneNumber { get; set; } = string.Empty;
}
