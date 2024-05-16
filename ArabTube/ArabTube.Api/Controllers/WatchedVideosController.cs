using ArabTube.Entities.DtoModels.WatchedVideoDto;
using ArabTube.Entities.Models;
using ArabTube.Services.CloudServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using ArabTube.Services.VideoServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static FFmpeg.NET.MetaData;

namespace ArabTube.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WatchedVideosController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public WatchedVideosController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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
                    var watchedVideos = await _unitOfWork.WatchedVideo.GetWatchedVideosAsync(user.Id);
                    if (watchedVideos != null)
                    {
                        var HistoryVideos = watchedVideos.Select(wv => new HistoryVideoDto
                        {
                            VideoId = wv.VideoId,
                            Title = wv.Video.Title,
                            Views = wv.Video.Views,
                            WatchedTime = wv.WatchedTime,
                            Username = wv.Video.AppUser.UserName,
                            Thumbnail = wv.Video.Thumbnail
                        });
                        return Ok(HistoryVideos);
                    }
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
                    var result = await _unitOfWork.WatchedVideo.DeleteWatchedVideoAsync(videoId , user.Id);
                    if (!result)
                    {
                        return NotFound($"No Video With Id = {videoId}");
                    }
                        
                    await _unitOfWork.Complete();
                    return Ok("Video Removed Succesfully");
                }
            }
            return Unauthorized();    
        }

    }
}
