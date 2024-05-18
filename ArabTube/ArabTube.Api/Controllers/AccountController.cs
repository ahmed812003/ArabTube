using ArabTube.Entities.DtoModels.UserDTOs;
using ArabTube.Services.AuthServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using ArabTube.Entities.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using ArabTube.Entities.Enums;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using ArabTube.Entities.DtoModels.VideoDTOs;

namespace ArabTube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IAuthService _authService, UserManager<AppUser> userManager, IEmailSender emailSender, IUnitOfWork unitOfWork)
        {
            this._authService = _authService;
            this._userManager = userManager;
            this._emailSender = emailSender;
            this._unitOfWork = unitOfWork;
        }

        [HttpGet("searchNames")]
        public async Task<IActionResult> SearchVideoTitles(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query Cannot Be Empty");

            var names = await _unitOfWork.AppUser.SearchUsersNamesAsync(query);

            if (!names.Any())
                return NotFound("No name Found Matching The Search Query");

            return Ok(names);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchVideos(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query Cannot Be Empty");

            var users = await _unitOfWork.AppUser.SearchUsersAsync(query);

            if (!users.Any())
                return NotFound("No users Found Matching The Search Query");

            var usersView = users.Select(u => new UserViewDto
            {
                UserId = u.Id,
                UserName = u.UserName,
                ChannelTitle = $"{u.FirstName} {u.LastName}"
            });
            return Ok(usersView);
        }


        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationDto model)
        {
            var userId = Request.Cookies["UserId"];
            var validCode = Request.Cookies["Code"];

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid User Id");
            }

            if (string.IsNullOrEmpty(validCode) || validCode != model.UserCode)
            {
                return BadRequest("Invalid Code");
            }


            var result = await _authService.EmailConfirmationAsync(userId);

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            var IsHasDefultPlaylists = await _unitOfWork.Playlist.CheckDefultPlaylistsAsync(userId);
            if (!IsHasDefultPlaylists)
            {
                var playlistNames = PlaylistDefaultNames.PlaylistNames;
                foreach (var playlistName in playlistNames)
                {
                    var entity = new Playlist
                    {
                        Title = playlistName,
                        UserId = userId,
                        IsDefult = true
                    };
                    await _unitOfWork.Playlist.AddAsync(entity);
                    await _unitOfWork.Complete();
                }
            }

            return Ok(result.Message);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (!result.IsSuccesed)
                return BadRequest(result.Message);

            string userId = result.Message;
            var code = GenerateOTP();

            Response.Cookies.Append("UserId", userId, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(10).ToLocalTime()

            });

            Response.Cookies.Append("Code", code, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(10).ToLocalTime()

            });

            await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                $"Code To confirm your account {code}.");

            return Ok("Please confirm your account");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.message);

            return Ok(result);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok("You Logout Succesfully");
        }

        [HttpPost("ResendEmailConfirmation")]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if ( user == null)
            {
                return BadRequest("No User With This Email");
            }

            var code = GenerateOTP();

            Response.Cookies.Append("UserId", user.Id, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(10).ToLocalTime()

            });

            Response.Cookies.Append("Code", code, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(10).ToLocalTime()

            });

            await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                $"Code To confirm your account {code}.");

            return Ok("Check Your E-mail");
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ForgetPasswordAsync(model.Email);

            if (!result.IsSuccesed)
                return BadRequest(result.Message);

            string userId = result.Message;
            var code = GenerateOTP();

            Response.Cookies.Append("UserId", userId, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(10).ToLocalTime()

            });

            Response.Cookies.Append("Code", code, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(10).ToLocalTime()

            });

            await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                $"Code To Reset Password {code}.");

            return Ok("Check Your Email To Reset Password");
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            var userId = Request.Cookies["UserId"];
            var validCode = Request.Cookies["Code"];

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid User Id");
            }

            if (string.IsNullOrEmpty(validCode) || validCode != model.UserCode)
            {
                return BadRequest("Invalid Code");
            }

            var result = await _authService.ResetPasswordAsync(userId, model.newPassword);

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost("Addrole")]
        public async Task<IActionResult> AddRoleAsync(RoleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            if (!result.IsSuccesed)
                return BadRequest(result);

            return Ok(result);
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

        private string GenerateOTP()
        {
            var random = new Random();
            var code = random.Next(0, 1000000).ToString();
            return code;
        }

    }
}