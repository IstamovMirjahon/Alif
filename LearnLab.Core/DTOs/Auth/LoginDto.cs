using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace  LearnLab.Core.DTOs.Auth;

public class LoginDto
{
    [Required(ErrorMessage = "Phone number is reuqired.")]
    [RegularExpression("^998\\d{9}$", ErrorMessage = "Invalid phone number.")]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    [MaxLength(20, ErrorMessage = "Password must be at most 20 characters.")]
    public string Password { get; set; }
}
