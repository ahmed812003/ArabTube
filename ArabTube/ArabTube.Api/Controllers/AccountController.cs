using ArabTube.Entities.AuthModels;
using ArabTube.Entities.DtoModels.UserDTOs;
using ArabTube.Entities.Models;
using ArabTube.Services.ControllersServices.AuthServices.Interfaces;
using ArabTube.Services.ControllersServices.PlaylistServices.Interfaces;
using ArabTube.Services.ControllersServices.UserServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;

namespace ArabTube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {   
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IPlaylistService _playlistService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AccountController(IAuthService _authService, UserManager<AppUser> userManager,
                                 IEmailSender emailSender, IPlaylistService playlistService, IUserService userService)
        {
            this._authService = _authService;
            this._userManager = userManager;
            this._emailSender = emailSender;
            this._playlistService = playlistService;
            this._userService = userService;
        }

        //AutoMapped
        [HttpGet("searchNames")]
        public async Task<IActionResult> SearchChannels(string query)
        {
            var result = await _userService.GetChannelsNameAsync(query);
            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.names);
        }

        //AutoMapped
        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers(string query)
        {
            var result = await _userService.GetUsersAsync(query);

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.users);
        }

        [HttpGet("User")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var result = await _userService.GetChannelAsync(userId);
            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.user);
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetChannelsAsync();
            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Channels);
        }


        [Authorize]
        [HttpPost("ProfilePic")]
        public async Task<IActionResult> SetProfilePic([FromForm] SetProfilePicDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _userService.SetProfilePicAsync(model , user);
                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }
                    return Ok("Your pic set sucessfully");
                }
            }
            return Unauthorized();
        }

        //AutoMapped
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationDto model)
        {
            var userId = model.UserId;
            var validCode = model.ValidCode;

            var result = await _authService.EmailConfirmationAsync(userId , validCode , model.UserCode);

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            var processResult = await _playlistService.CreateDefultPlaylistsAsync(userId);

            if (!processResult.IsSuccesed)
            {
                return BadRequest(processResult.Message);
            }

            return Ok(result.Message);
        }

        //AutoMapped
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (!result.IsSuccesed)
                return BadRequest(result.Message);
            

            await _emailSender.SendEmailAsync(model.Email, EmailCotent.ConfirmEmailSubject,
                $"{EmailCotent.ConfirmEmailHtmlMessage}{result.SecurityResponse.Code}.");

            return Ok(result.SecurityResponse);
        }
        
        //AutoMapped
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

        //AutoMapped
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok("You Logout Succesfully");
        }

        //AutoMapped 
        [HttpPost("ResendEmailConfirmation")]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ResendEmailConfirmationAsync(model);

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            await _emailSender.SendEmailAsync(model.Email, EmailCotent.ConfirmEmailSubject,
               $"{EmailCotent.ConfirmEmailHtmlMessage}{result.SecurityResponse.Code}.");

            return Ok(result.SecurityResponse);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ForgetPasswordAsync(model.Email);

            if (!result.IsSuccesed)
                return BadRequest(result.Message);

            await _emailSender.SendEmailAsync(model.Email, EmailCotent.ResetPasswordSubject,
                $"{EmailCotent.ResetPasswordHtmlMessage}{result.SecurityResponse.Code}.");

            return Ok(result.SecurityResponse);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            var userId = model.UserId;
            var validCode = model.ValidCode;

            var result = await _authService.ResetPasswordAsync(userId, validCode , model.UserCode, model.newPassword);

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
    }
}