using ArabTube.Entities.DtoModels.WatchedVideoDto;
using ArabTube.Entities.Models;
using ArabTube.Services.ControllersServices.WatchedVideoServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArabTube.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WatchedVideosController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IWatchedVideoService _watchedVideoService;

        public WatchedVideosController(UserManager<AppUser> userManager, IWatchedVideoService watchedVideoService)
        {
            _userManager = userManager;
            _watchedVideoService = watchedVideoService;
        }

        [HttpGet]
        public async Task<IActionResult> WatchedVideos()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _watchedVideoService.GetWatchedVideosAsync(user.Id);
                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }
                    return Ok(result.Videos);

                }
            }
            return Unauthorized();
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(string videoId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    if (user.Isbaneed)
                    {
                        return BadRequest("You Are banned");
                    }
                    var result = await _watchedVideoService.RemoveFromHistoryAsync(videoId, user.Id);
                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }
                    return Ok("Video Removed Succesfully");
                }
            }
            return Unauthorized();    
        }

    }
}
