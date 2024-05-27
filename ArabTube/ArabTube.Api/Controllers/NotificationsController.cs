using ArabTube.Entities.Models;
using ArabTube.Services.ControllersServices.NotificationServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArabTube.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly INotificationService _notificationService;

        public NotificationsController(UserManager<AppUser> userManager, INotificationService notificationService)
        {
            this._userManager = userManager;
            _notificationService = notificationService;
        }

        [HttpGet("Notifications")]
        public async Task<IActionResult> Notifications()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _notificationService.NotificationsAsync(user.Id);
                    if (!result.IsSuccesed)
                        return BadRequest(result.Message);
                    return Ok(result.NotificationsDtos);
                }
            }
            return Unauthorized();
        }
    }
}
