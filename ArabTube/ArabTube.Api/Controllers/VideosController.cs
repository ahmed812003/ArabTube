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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ArabTube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {   
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVideoService _videoService;
        private readonly IPlaylistService _playlistService;
        private readonly ICommentService _commentService;
        private readonly IPlaylistVideoService _playlistVideoService;
        private readonly IWatchedVideoService _watchedVideoService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public VideosController(IUnitOfWork unitOfWork, IVideoService videoService, IPlaylistService playlistService, 
                                ICommentService commentService, IPlaylistVideoService playlistVideoService, 
                                IWatchedVideoService watchedVideoService, UserManager<AppUser> userManager, 
                                IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _videoService = videoService;
            _playlistService = playlistService;
            _commentService = commentService;
            _playlistVideoService = playlistVideoService;
            _watchedVideoService = watchedVideoService;
            _userManager = userManager;
            _mapper = mapper;
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

            var result = await _videoService.UploadVideoAsync(model,userName);
            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            await _unitOfWork.Complete();
            return Ok("Video Uploaded Sucessfully");
        }

        // AutoMapped
        // dont forget to handle Save changes 
        [Authorize]
        [HttpPost("Like")]
        public async Task<IActionResult> LikeVideo(string id)
        {
            var result = await _videoService.LikeVideoAsync(id);

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }
     
            var playlistId = await _playlistService.GetPlaylistIdAsync(PlaylistDefaultNames.PlaylistNames[0], true);

            var isVideoAdded = await _playlistVideoService.AddVideoToPlayListAsync(id, playlistId);
            if (!isVideoAdded.IsSuccesed)
            {
                return BadRequest(isVideoAdded.Message);
            }

            return Ok($"User has Liked Video successfully!");
        }

        // dont forget to handle Save changes 
        [Authorize]
        [HttpPost("Dislike")]
        public async Task<IActionResult> DislikeVideo(string videoId)
        {
            var result = await _videoService.DislikeVideoAsync(videoId);

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            var playlistId = await _playlistService.GetPlaylistIdAsync(PlaylistDefaultNames.PlaylistNames[0], true);
            result = await _playlistVideoService.RemoveVideoFromPlaylistAsync(videoId, playlistId);
            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            return Ok($"Video has Disliked successfully ");
        }

        // dont forget to handle Save changes 
        [Authorize]
        [HttpPost("Flag")]
        public async Task<IActionResult> FlagVideo(string id)
        {
            var result = await _videoService.FlagVideoAsync(id);
            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }
            return Ok("User Flaged video successfully");
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

                    var isWatchedVideoAdded = await _watchedVideoService.AddWatchedVideoToHistoryAsync(user.Id, id);
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
        public async Task<IActionResult> UpdateVideo(UpdatingVideoDto model, string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _videoService.UpdateVideoAsync(model, id);
            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }
            return Ok("Video Updated Sucessfully");
        }

        [Authorize]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteVideo(string id)
        {
            var result = await _commentService.DeleteVideoCommentsAsync(id);
            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }
            result = await _videoService.DeleteAsync(id);
            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }
            return Ok("Video Deleted Successfully");
        }
    
    }
}
  