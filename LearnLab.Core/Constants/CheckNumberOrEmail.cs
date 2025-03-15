using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LearnLab.Core.Constants
{
    public static class CheckNumberOrEmail
    {
        public static bool IsPhoneNumber(string EmailOrPhone)
        {
            return Regex.IsMatch(EmailOrPhone, "^998\\d{9}$");
        }

        public static bool IsEmail(string EmailOrPhone)
        {
            return new EmailAddressAttribute().IsValid(EmailOrPhone);
        }
    }
}
