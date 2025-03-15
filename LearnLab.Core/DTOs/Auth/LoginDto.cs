using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LearnLab.Core.DTOs.Auth;

public class LoginDto
{
    [Required(ErrorMessage = "Email yoki telefon raqami majburiy.")]
    public string EmailOrPhone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parol majburiy.")]
    [MinLength(8, ErrorMessage = "Parol kamida 8 ta belgidan iborat bo‘lishi kerak.")]
    [MaxLength(20, ErrorMessage = "Parol eng ko‘pi bilan 20 ta belgidan iborat bo‘lishi kerak.")]
    public string Password { get; set; } = string.Empty;

}

