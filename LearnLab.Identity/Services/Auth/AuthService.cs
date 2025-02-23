using LearnLab.Core.AccesConfigurations;
using LearnLab.Core.Constants;
using LearnLab.Core.DTOs.Auth;
using LearnLab.Core.Exceptions;
using LearnLab.Identity.Constants;
using LearnLab.Identity.Models;
using LearnLab.Identity.SMS;
using Microsoft.AspNetCore.Identity;
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
        var user = await _userManager.FindByNameAsync(loginDto.PhoneNumber) ??
            throw new LearnLabException("Not Found");

        if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            throw new LearnLabException("Your Password is incorrect.");

        if (!user.PhoneNumberConfirmed)
        {
            await _smsSender.SendSmsOtpAsync(user.PhoneNumber);
            throw new LearnLabException($"Please verify your phone number. {user.PhoneNumber}");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.PhoneNumber),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimNames.UserId, Convert.ToString(user.Id)),
            new Claim(ClaimNames.FirstName, user.FirstName),
            new Claim(ClaimNames.LastName, user.LastName)
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey.TheSecretKey));

        // add roels to claims
        foreach (var role in roles)
        {
            var roleClaim = new Claim(ClaimTypes.Role, role);
            authClaims.Add(roleClaim);
        }

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _siteSettings.Value.Issuer,
            audience: _siteSettings.Value.Audience,
            expires: DateTime.Now.AddDays(10),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return new LoginResponseDto(
                        Guid.Parse(user.Id), token, jwtSecurityToken.ValidTo,
                        user.FirstName, user.LastName, user.Gender,
                        user.BirthDate, user.PhoneNumber, roles);
    }

    public async Task<SignUpResponseDto> Register(SignUpDto signUpDto)
    {
        var user = await _userManager.FindByNameAsync(signUpDto.PhoneNumber);

        if (user != null && user.PhoneNumberConfirmed == true)
            throw new LearnLabException("This user already created. You can Login to your account.");

        if (user != null && user.PhoneNumberConfirmed == false)
            await _userManager.DeleteAsync(user);

        var newUser = new User(signUpDto.FirstName, signUpDto.LastName, signUpDto.PhoneNumber);

        var result = await _userManager.CreateAsync(newUser, signUpDto.Password);

        if (!result.Succeeded)
            throw new LearnLabException("Didn't Succeed.");

        var roles = new List<string> { RoleNames.Guest, signUpDto.Role };

        await _userManager.AddToRolesAsync(newUser, roles.ToArray());

        await _smsSender.SendSmsOtpAsync(signUpDto.PhoneNumber);

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
