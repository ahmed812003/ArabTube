using ArabTube.Entities.DtoModels.VideoDTOs;
using ArabTube.Entities.Enums;
using ArabTube.Entities.Models;
using ArabTube.Entities.VideoModels;
using ArabTube.Services.CloudServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using ArabTube.Services.VideoServices.Interfaces;
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
        private readonly IVideoService _videoService;
        private readonly ICloudService _cloudService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        public VideosController(IVideoService videoService, IUnitOfWork unitOfWork, ICloudService cloudService,
            UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _videoService = videoService;
            _unitOfWork = unitOfWork;
            _cloudService = cloudService;
            _userManager = userManager;
            _configuration = configuration;
        }
        // AutoMapped
        [HttpGet("searchTitles")]
        public async Task<IActionResult> SearchVideoTitles(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query Cannot Be Empty");

            var titles = await _videoService.SearchVideoTitlesAsync(query);

            if (!titles.Any())
                return NotFound("No Titles Found Matching The Search Query");

            return Ok(titles);
        }

        // AutoMapped
        [HttpGet("search")]
        public async Task<IActionResult> SearchVideos(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query Cannot Be Empty");

            var viewVideos = await _videoService.SearchVideoAsync(query);

            if (!viewVideos.Any())
                return NotFound("No Videos Found Matching The Search Query");

            return Ok(viewVideos);
        }
        // AutoMapped
        [HttpGet("Videos")]
        public async Task<IActionResult> PreviewVideo()
        {
            var viewVideos = await _videoService.GetAllAsync();
         
            if (!viewVideos.Any())
                return NotFound("No Videos Found Matching The Search Query");

            return Ok(viewVideos);
        }

        // AutoMapped
        [HttpGet("Video")]
        public async Task<IActionResult> WatchVideo(string id)
        {
            var viewVideo = await _videoService.FindByIdAsync(id);

            if (viewVideo == null)
            {
                return BadRequest($"Video With id {id} does not exist!");
            }

            return Ok(viewVideo);
        }

        [Authorize]
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadVideo([FromForm] UploadingVideoDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            if (model.Video.ContentType != "video/mp4")
            {
                return BadRequest("Video Type is Not Mp4");
            }

            string? userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByNameAsync(userName);

            var processingVideo = new ProcessingVideo
            {
                Username = userName, 
                Video = model.Video,
                Title = model.Title
            };

           /*var encodedVideos = await _videoService.ProcessVideoAsync(processingVideo);
             await _cloudService.UploadToCloudAsync(encodedVideos);*/

            var result = await _videoService.AddAsync(model,userName);
            if (!result)
            {
                return BadRequest("Failed to Add ");
            }
            return Ok("Video Uploaded Sucessfully");
        }

        [Authorize]
        [HttpPost("Like")]
        public async Task<IActionResult> LikeVideo(string id)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);

            if (video == null)
            {
                return NotFound($"Video With id {id} does not exist!");
            }

            video.Likes += 1;
            var playlistId = await _unitOfWork.Playlist.FindPlaylistByNameAsync(PlaylistDefaultNames.PlaylistNames[0], true);
            await _unitOfWork.PlaylistVideo.AddVideoToPlayListAsync(id, playlistId);
            await _unitOfWork.Complete();

            return Ok($"Video Likes = {video.Likes}");
        }

        [Authorize]
        [HttpPost("Dislike")]
        public async Task<IActionResult> DislikeVideo(string id)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);

            if (video == null)
            {
                return NotFound($"Video With id {id} does not exist!");
            }

            video.DisLikes += 1;
            var playlistId = await _unitOfWork.Playlist.FindPlaylistByNameAsync(PlaylistDefaultNames.PlaylistNames[0], true);
            var result = await _unitOfWork.PlaylistVideo.FindVideoInPlaylist(id, playlistId);
            if (result)
            {
                await _unitOfWork.PlaylistVideo.RemoveVideoFromPlayListAsync(id, playlistId);
            }

            await _unitOfWork.Complete();
            return Ok($"Video Dislikes = {video.DisLikes}");
        }

        [Authorize]
        [HttpPost("Flag")]
        public async Task<IActionResult> FlagVideo(string id)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);

            if (video == null)
            {
                return NotFound($"Video With id {id} does not exist!");
            }

            video.Flags += 1;
            await _unitOfWork.Complete();

            return Ok($"Video Flags = {video.Flags}");
        }

        [Authorize]
        [HttpPost("View")]
        public async Task<IActionResult> ViewVideo(string id)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);

            if (video == null)
            {
                return NotFound($"Video With id {id} does not exist!");
            }

            video.Views += 1;
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var isWatchedVideoAdded = await _unitOfWork.WatchedVideo.AddWatchedVideoToHistoryAsync(user.Id, id);
                }
            }

            await _unitOfWork.Complete();

            return Ok($"Video views = {video.Views}");
        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateVideo(UpdatingVideoDto model, string id)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);

            if (video == null)
            {
                return NotFound($"Video With id {id} does not exist!");
            }

            if (!string.IsNullOrEmpty(model.Title))
                video.Title = model.Title;
            if (!string.IsNullOrEmpty(model.Description))
                video.Description = model.Description;
            if (model.Thumbnail != null)
            {
                using var stream = new MemoryStream();
                await model.Thumbnail.CopyToAsync(stream);
                video.Thumbnail = stream.ToArray();
            }

            await _unitOfWork.Complete();
            return Ok("Video Updated Sucessfully");
        }

        [Authorize]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteVideo(string id)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);
            if(video == null)
            {
                return NotFound($"Video With id {id} does not exist!");
            }
            await _unitOfWork.Comment.DeleteVideoCommentsAsync(id);
            await _unitOfWork.Video.DeleteAsync(id);
            await _unitOfWork.Complete();
            return Ok("Video Deleted Successfully");
        }
    }
}
