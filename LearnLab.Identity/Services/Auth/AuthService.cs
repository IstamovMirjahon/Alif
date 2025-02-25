using LearnLab.Core.AccesConfigurations;
using LearnLab.Core.Constants;
using LearnLab.Core.DTOs.Auth;
using LearnLab.Core.Exceptions;
using LearnLab.Identity.Constants;
using LearnLab.Identity.Email;
using LearnLab.Identity.Models;
using LearnLab.Identity.SMS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetShop.Core.AccesConfigurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LearnLab.Identity.Services.Auth;

public class AuthService : IAuthService
{
    private IOptions<AccessConfiguration> _siteSettings;
    private readonly UserManager<User> _userManager;
    private readonly LearnLabIdentityDbContext _context;
    private readonly ISmsSender _smsSender;
    private readonly IEmailSender _emailSender;

    public AuthService(
                IOptions<AccessConfiguration> siteSettings,
                UserManager<User> userManager,
                LearnLabIdentityDbContext context,
                ISmsSender smsSender)
    {   
        _siteSettings = siteSettings;
        _userManager = userManager;
        _context = context;
        _smsSender = smsSender;
    }

    public async Task<LoginResponseDto> Login(LoginDto loginDto)
    {
        User? user = null;

        if (loginDto.IsPhoneNumber())
        {
            user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == loginDto.EmailOrPhone);
        }
        else if (loginDto.IsEmail())
        {
            user = await _userManager.FindByEmailAsync(loginDto.EmailOrPhone);
        }

        if (user == null)
            throw new LearnLabException("Foydalanuvchi topilmadi.");

        if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            throw new LearnLabException("Noto‘g‘ri parol.");

        if (!user.PhoneNumberConfirmed)
        {
            await _smsSender.SendSmsOtpAsync(user.PhoneNumber);
            throw new LearnLabException($"Iltimos, telefon raqamingizni tasdiqlang: {user.PhoneNumber}");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimNames.FirstName, user.FirstName),
        new Claim(ClaimNames.LastName, user.LastName)
    };

        foreach (var role in roles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey.TheSecretKey));
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _siteSettings.Value.Issuer,
            audience: _siteSettings.Value.Audience,
            expires: DateTime.UtcNow.AddDays(10),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return new LoginResponseDto(
            Guid.Parse(user.Id),
            token,
            jwtSecurityToken.ValidTo,
            user.FirstName,
            user.LastName,
            user.Gender,
            user.BirthDate,
            user.PhoneNumber,
            roles,
            user.Email
        );
    }

    public async Task<SignUpResponseDto> Register(SignUpDto signUpDto)
    {
        User? existingUser = null;

        if (!string.IsNullOrEmpty(signUpDto.Email))
        {
            existingUser = await _userManager.FindByEmailAsync(signUpDto.Email);
        }
        else if (!string.IsNullOrEmpty(signUpDto.PhoneNumber))
        {
            existingUser = await _userManager.FindByNameAsync(signUpDto.PhoneNumber);
        }

        if (existingUser != null && existingUser.PhoneNumberConfirmed)
            throw new LearnLabException("This user already exists. You can login to your account.");

        if (existingUser != null && !existingUser.PhoneNumberConfirmed)
            await _userManager.DeleteAsync(existingUser);

        var newUser = new User(
            signUpDto.FirstName,
            signUpDto.LastName,
            signUpDto.PhoneNumber,
            signUpDto.Email,
            signUpDto.Gender,
            DateTime.MinValue
        );

        var result = await _userManager.CreateAsync(newUser, signUpDto.Password);

        if (!result.Succeeded)
            throw new LearnLabException("User registration failed.");

        var roles = new List<string> { RoleNames.Guest, signUpDto.Role };
        await _userManager.AddToRolesAsync(newUser, roles.ToArray());

        if (!string.IsNullOrEmpty(signUpDto.PhoneNumber))
        {
            await _smsSender.SendSmsOtpAsync(signUpDto.PhoneNumber);
        }
        else if (!string.IsNullOrEmpty(signUpDto.Email))
        {
            await _emailSender.SendEmailOtpAsync(signUpDto.Email);
        }

        return new SignUpResponseDto(Guid.Parse(newUser.Id), newUser.PhoneNumber, newUser.FirstName, newUser.LastName, roles);
    }


    public async Task<bool> VerifyPhoneNumber(string phoneNumber, string user_code)
    {
        var user = await _userManager.FindByNameAsync(phoneNumber) ??
            throw new LearnLabException("User not found.");

        var code = _context.SmsTokens.Where(x => x.PhoneNumber == phoneNumber).FirstOrDefault()?.SmsCode ??
            throw new LearnLabException("Code not found.");

        if (code != user_code)
            throw new LearnLabException("Code is incorrect.");

        user.PhoneNumberConfirmed = true;
        await _userManager.UpdateAsync(user);

        return true;
    }

    public async Task<bool> ForgetPassword(string phoneNumber)
    {
        var user = await _userManager.FindByNameAsync(phoneNumber) ??
            throw new LearnLabException("User not found.");

        await _smsSender.SendSmsOtpAsync(user.PhoneNumber);

        return true;
    }

    public async Task<bool> ResetPassword(string phoneNumber, string newPassword, string code)
    {
        var user = await _userManager.FindByNameAsync(phoneNumber) ??
            throw new LearnLabException("User not found.");

        var smsToken = _context.SmsTokens.Where(x => x.PhoneNumber == phoneNumber).FirstOrDefault() ??
            throw new LearnLabException("Code not found.");

        if (smsToken.SmsCode != code)
            throw new LearnLabException("Code is incorrect.");

        await _userManager.RemovePasswordAsync(user);
        await _userManager.AddPasswordAsync(user, newPassword);

        return true;
    }
}
