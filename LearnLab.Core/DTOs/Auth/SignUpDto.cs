using LearnLab.Core.Attributes;
using LearnLab.Core.Enum.GenderTypes;
using System.ComponentModel.DataAnnotations;

namespace LearnLab.Core.DTOs.Auth;

public class SignUpDto
{
    [RegularExpression("^998\\d{9}$", ErrorMessage = "Please enter a valid phone number starting with 998 and 12 digits long.")]
    public string? PhoneNumber { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string? Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "This field is Required.")]
    [StringLength(100, ErrorMessage = "Minimum Length = 8 !", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "This field is Required.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match!")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "This field is Required.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "This field is Required.")]
    public string LastName { get; set; } = string.Empty;
    public GenderEnum Gender { get; set; } = GenderEnum.Unknown;

    [Required(ErrorMessage = "This field is Required.")]
    [AllowedRoles]
    public string Role { get; set; } = string.Empty;
}
