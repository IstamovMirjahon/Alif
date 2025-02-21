using System.ComponentModel.DataAnnotations;
namespace LearnLab.Core.DTO.Auth;

public class ResetPasswordByCodeDto
{
    [RegularExpression("^998\\d{9}$", ErrorMessage = "Please enter a valid phone number starting with 998 and 12 digits long.")]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Please enter the new passpowrd.")]
    public string Newpassword { get; set; }
    [Required(ErrorMessage = "You have to enter verifiaction code.")]
    public string Code { get; set; }

}
