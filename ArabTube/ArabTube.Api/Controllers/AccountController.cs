using ArabTube.Entities.DtoModels.UserDTOs;
using ArabTube.Services.AuthServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using ArabTube.Entities.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ArabTube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AccountController(IAuthService _authService, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            this._authService = _authService;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpGet("ConfirmEmail/{userId}&{code}")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var result = await _authService.EmailConfirmationAsync(userId, code);

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);
            try
            {
                var result = await _authService.RegisterAsync(model);

                if (!result.IsSuccesed)
                    return BadRequest(result.Message);

                var callbackUrl = await GenerateConfirmEmailUrl(model.Email);
                var encodedUrl = HtmlEncoder.Default.Encode(callbackUrl);
                await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{encodedUrl}'>clicking here</a>.");

                return Ok("Please confirm your account");
            }
            catch
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var deleteResult = await _userManager.DeleteAsync(user);
                if(!deleteResult.Succeeded)
                {
                    return BadRequest("Not Deleted");
                }
                return BadRequest("Register Failed, Please Register Again!");
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.message);

            return Ok(result);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email Is Not Found");

            var user =await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("No User With This Email");

            var callbackUrl = await GenerateResetPasswordUrl(user.Id);
            var encodedUrl = HtmlEncoder.Default.Encode(callbackUrl);
            await _emailSender.SendEmailAsync(email, "Reset Password",
                $"To Reset Password <a href='{encodedUrl}'>clicking here</a>.");

            return Ok("Check Your Email To Reset Password"); 
        }

        [HttpPost("ResetPassword/{userId}&{code}")]
        public async Task<IActionResult> ResetPassword (string userId,string code, string newPassword)
        {
            var result =await _authService.ResetPasswordAsync(userId, code, newPassword);

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost("Addrole")]
        public async Task<IActionResult> AddRoleAsync(RoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

        private async Task<string> GenerateConfirmEmailUrl(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Request.Scheme + "://" + Request.Host +
                                    Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code });
            return callbackUrl;
        }

        private async Task<string> GenerateResetPasswordUrl(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Request.Scheme + "://" + Request.Host +
                                    Url.Action("ResetPassword", "Account", new { userId = userId , code = code});
            return callbackUrl;
        }

    }
}