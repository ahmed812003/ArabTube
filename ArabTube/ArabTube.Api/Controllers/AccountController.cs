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

namespace ArabTube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public AccountController(IAuthService _authService, UserManager<AppUser> userManager, IEmailSender emailSender, IMapper mapper)
        {
            this._authService = _authService;
            _userManager = userManager;
            _emailSender = emailSender;
            _mapper = mapper;
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

                var callbackUrl = await GenerateMailMessage(model.Email);

                await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return Ok("Please confirm your account");
            }
            catch (Exception ex)
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

        private async Task<String> GenerateMailMessage (string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            return callbackUrl;
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
    }
}
