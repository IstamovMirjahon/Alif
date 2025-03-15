using LearnLab.Core.DTOs.Auth;

namespace LearnLab.Identity.Services.Auth;

public interface IAuthService
{
    Task<LoginResponseDto> Login(LoginDto loginDto);
    Task<SignUpResponseDto> Register(SignUpDto registerDto);
    Task<bool> VerifyPhoneNumber(string phoneNumber, string userCode);
    Task<bool> VerifyEmail(string email, string user_code);
    Task<bool> ForgetPassword(string phoneNumber);
    Task<bool> ForgetPasswordByEmail(string email);
    Task<bool> ResetPassword(string phoneNumber, string newPassword, string code);
    Task<bool> ResetEmail(string email, string newPassword, string code);
}
