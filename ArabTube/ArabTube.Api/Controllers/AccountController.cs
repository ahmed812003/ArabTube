using ArabTube.Entities.DtoModels.UserDTOs;
using ArabTube.Services.AuthServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using ArabTube.Entities.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using ArabTube.Entities.AuthModels;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;
using AutoMapper;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Drawing.Printing;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;

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

        [Route("ConfirmEmail")]
        [HttpGet()]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest("Invalid Email");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            string statusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            return Ok(statusMessage);
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);
            try
            {
                var result = await _authService.RegisterAsync(model);

                if (!result.IsCreated)
                    return BadRequest(result.message);

                var callbackUrl = await GenerateConfirmEmailMessage(model.Email);

                await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

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


        [HttpPost("Addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] RoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }


        private async Task<String> GenerateConfirmEmailMessage(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Request.Scheme + "://" + Request.Host +
                                    Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code });
            return callbackUrl;
        }

    }
}
