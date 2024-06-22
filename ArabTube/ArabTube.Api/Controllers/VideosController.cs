using ArabTube.Entities.DtoModels.VideoDTOs;
using ArabTube.Entities.Enums;
using ArabTube.Entities.Models;
using ArabTube.Entities.VideoModels;
using ArabTube.Services.ControllersServices.CommentServices.Interfaces;
using ArabTube.Services.ControllersServices.PlaylistServices.Interfaces;
using ArabTube.Services.ControllersServices.PlaylistVideoServices.Interfaces;
using ArabTube.Services.ControllersServices.VideoServices.Interfaces;
using ArabTube.Services.ControllersServices.WatchedVideoServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using AutoMapper;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArabTube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly IPlaylistService _playlistService;
        private readonly ICommentService _commentService;
        private readonly IPlaylistVideoService _playlistVideoService;
        private readonly IWatchedVideoService _watchedVideoService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public VideosController(IVideoService videoService, IPlaylistService playlistService,
                                ICommentService commentService, IPlaylistVideoService playlistVideoService,
                                IWatchedVideoService watchedVideoService, UserManager<AppUser> userManager,
                                IConfiguration configuration)
        {
            _videoService = videoService;
            _playlistService = playlistService;
            _commentService = commentService;
            _playlistVideoService = playlistVideoService;
            _watchedVideoService = watchedVideoService;
            _userManager = userManager;
            _configuration = configuration;
        }

        // AutoMapped
        [HttpGet("searchTitles")]
        public async Task<IActionResult> SearchVideoTitles(string query)
        {
            var result = await _videoService.SearchVideoTitlesAsync(query);

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Titles);
        }

        // AutoMapped
        [HttpGet("search")]
        public async Task<IActionResult> SearchVideos(string query)
        {
            var result = await _videoService.SearchVideoAsync(query);
            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Videos);
        }

        // AutoMapped
        [HttpGet("Videos")]
        public async Task<IActionResult> PreviewVideo()
        {
            var result = await _videoService.PreviewVideoAsync();

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Videos);
        }

        // AutoMapped
        [HttpGet("Video")]
        public async Task<IActionResult> WatchVideo(string id)
        {
            var result = await _videoService.WatchVideoAsync(id);

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Video);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("FlagedVideos")]
        public async Task<IActionResult> GetFlagedVideos()
        {
            var result = await _videoService.GetFlagedVideosAsync();

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.viewVideoDtos);
        }

        [HttpGet("UserVideo")]
        public async Task<IActionResult> UserVideos(string userId)
        {
            var result = await _videoService.UserVideosAsync(userId);
            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Videos);
        }

        [Authorize]
        [HttpGet("UserLike")]
        public async Task<IActionResult> IsUserLikeVideo(string videoId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _videoService.IsUserLikeVideoAsync(videoId, user.Id);
                    if(!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);  
                    }
                    return Ok(result.Message);
                }
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpGet("UserDisLike")]
        public async Task<IActionResult> IsUserDisLikeVideo(string videoId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _videoService.IsUserDisLikeVideoAsync(videoId, user.Id);
                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }
                    return Ok(result.Message);
                }
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpGet("UserFlag")]
        public async Task<IActionResult> IsUserFlagVideo(string videoId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _videoService.IsUserFlagVideoAsync(videoId, user.Id);
                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }
                    return Ok(result.Message);
                }
            }
            return Unauthorized();
        }

        // AutoMapped
        [Authorize]
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadVideo([FromForm] UploadingVideoDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            string? userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
            {
                return Unauthorized();
            }
            var user = await _userManager.FindByNameAsync(userName);
            if(user == null)
            {
                return Unauthorized();
            }
            if (user.Isbaneed)
            {
                return BadRequest("You Are banned");
            }
            var result = await _videoService.UploadVideoAsync(model,userName , user);
            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            return Ok("Video Uploaded Sucessfully");
        }

        // AutoMapped
        [Authorize]
        [HttpPost("Like")]
        public async Task<IActionResult> LikeVideo(string id)
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
                    var result = await _videoService.LikeVideoAsync(id , user.Id);

                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }

                    var playlistId = await _playlistService.GetPlaylistIdAsync(PlaylistDefaultNames.PlaylistNames[0], true, user.Id);
                    if (result.Message == "Add")
                    {
                        result = await _playlistVideoService.AddVideoToPlayListAsync(id, playlistId, user.Id);
                        if (!result.IsSuccesed)
                        {
                            return BadRequest(result.Message);
                        }
                    }
                    else
                    {
                        result = await _playlistVideoService.RemoveVideoFromPlaylistAsync(id, playlistId, user.Id);
                        if (!result.IsSuccesed)
                        {
                            return BadRequest(result.Message);
                        }
                    }

                    return Ok($"User has Liked Video successfully!");
                }
            }
            return Unauthorized();        
        }

        [Authorize]
        [HttpPost("Dislike")]
        public async Task<IActionResult> DislikeVideo(string videoId)
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
                    var result = await _videoService.DislikeVideoAsync(videoId , user.Id);

                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }

                    return Ok($"Video has Disliked successfully ");
                }
            }
            return Unauthorized();        
        }
 
        [Authorize]
        [HttpPost("Flag")]
        public async Task<IActionResult> FlagVideo(string id)
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
                    var result = await _videoService.FlagVideoAsync(id , user.Id);
                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }
                    return Ok("User Flaged video successfully");
                }
            }
            return Unauthorized();        
        }

        [Authorize]
        [HttpPost("View")]
        public async Task<IActionResult> ViewVideo(string id)
        {            
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _videoService.ViewVideoAsync(id);
                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }

                    var isWatchedVideoAdded = await _watchedVideoService.AddWatchedVideoToHistoryAsync(user.Id , id);
                    if(!isWatchedVideoAdded.IsSuccesed)
                    {
                        return BadRequest(isWatchedVideoAdded.Message);
                    }

                    return Ok("User has watched video successfully");
                }
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateVideo([FromForm]UpdatingVideoDto model, string id)
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
                    if (user.Isbaneed)
                    {
                        return BadRequest("You Are banned");
                    }
                    var result = await _videoService.UpdateVideoAsync(model, id , user.Id);
                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }
                    return Ok("Video Updated Sucessfully");
                }
            }
            return Unauthorized();
                    
        }

        [Authorize]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteVideo(string id)
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
                    var result = await _commentService.DeleteVideoCommentsAsync(id , user);
                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }

                    result = await _videoService.DeleteAsync(id , user);
                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }

                    return Ok("Video Deleted Successfully");
                }
            }
            return Unauthorized();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("FlagVideo")]
        public async Task<IActionResult> DeleteFlagVideos(string videoId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _videoService.DeleteFlagVideoAsync(videoId, user);
                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }

                    return Ok("Video Deleted Successfully");
                }
            }
            return Unauthorized();
        }

    }
}
  