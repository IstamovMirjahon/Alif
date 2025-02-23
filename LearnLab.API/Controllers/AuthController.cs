using LearnLab.Core.DTO.Auth;
using LearnLab.Core.DTOs.Auth;
using LearnLab.Identity.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<LoginResponseDto>> LoginAsync([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("LoginDto can not be null.");
            }
            return Ok(await _authService.Login(loginDto));
        }

        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult<SignUpResponseDto>> SignUpAsync([FromBody] SignUpDto signUpDto)
        {
            return Ok(await _authService.Register(signUpDto));
        }

        [HttpPost]
        [Route("verify-phone")]
        public async Task<IActionResult> VerifyPhoneNumber([FromBody] VerifyPhoneNumberDto verifyDto)
        {
            if (await _authService.VerifyPhoneNumber(verifyDto.PhoneNumber, verifyDto.Code))
                return Ok("Successfull!");
            return BadRequest("Not Saccessfull!");
        }

        [HttpPost]
        [Route("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto forgetPasswordDto)
        {
            if (await _authService.ForgetPassword(forgetPasswordDto.PhoneNumber))
                return Ok("Successfull!");
            return BadRequest("Not Saccessfull!");
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordByCodeDto resetPasswordDto)
        {
            if (await _authService.ResetPassword(resetPasswordDto.PhoneNumber, resetPasswordDto.Newpassword, resetPasswordDto.Code))
                return Ok("Successfull!");
            return BadRequest("Not Saccessfull!");
        }
    }
}
