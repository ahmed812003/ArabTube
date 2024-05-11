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

        [HttpGet("Videos")]
        public async Task<IActionResult> PreviewVideo()
        {
            var videos = await _unitOfWork.Video.GetAllAsync();

            var viewVideos = videos.Select(v => new VideoPreviewDto
            {
                Id = v.Id,
                Title = v.Title,
                Likes = v.Likes,
                DisLikes = v.DisLikes,
                Views = v.Views,
                CreatedOn = v.CreatedOn,
                Thumbnail = v.Thumbnail
            });

            return Ok(viewVideos);
        }

        [HttpGet("Video/{id}")]
        public async Task<IActionResult> WatchVideo(string id)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);

            if (video == null)
            {
                return BadRequest($"Video With id {id} does not exist!");
            }

            var viewVideo = new ViewVideoDto
            {
                Title = video.Title,
                Description = video.Description,
                Likes = video.Likes,
                DisLikes = video.DisLikes,
                Views = video.Views,
                Flags = video.Flags,
                UpdatedOn = video.UpdatedOn,
                Thumbnail = video.Thumbnail
            };
            var resolutions = Enum.GetValues(typeof(VideoResolutions));
            foreach (var resolution in resolutions)
            {
                viewVideo.VideoUriList.Add($"{video.VideoUri}{(int)resolution}");
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

            var encodedVideos = await _videoService.ProcessVideoAsync(processingVideo);

            await _cloudService.UploadToCloudAsync(encodedVideos);

            using var stream = new MemoryStream();
            await model.Thumbnail.CopyToAsync(stream);

            string uri = new Uri(_configuration["BlobStorage:ConnectionString"]).ToString();

            var video = new Video
            {
                Title = model.Title,
                Description = model.Description,
                Thumbnail = stream.ToArray(),
                UserId = user.Id,
                VideoUri = $"{uri}{userName}/{model.Title}-"
            };

            await _unitOfWork.Video.AddAsync(video);
            await _unitOfWork.Complete();


            return Ok("Video Uploaded Sucessfully");
        }

        [HttpPost("Like/{id}")]
        public async Task<IActionResult> LikeVideo(string id)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);

            if (video == null)
            {
                return BadRequest($"Video With id {id} does not exist!");
            }

            video.Likes += 1;
            await _unitOfWork.Complete();

            return Ok($"Video Likes = {video.Likes}");
        }

        [HttpPost("Dislike/{id}")]
        public async Task<IActionResult> DislikeVideo(string id)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);

            if (video == null)
            {
                return BadRequest($"Video With id {id} does not exist!");
            }

            video.DisLikes += 1;
            await _unitOfWork.Complete();

            return Ok($"Video Dislikes = {video.DisLikes}");
        }

        [HttpPost("Flag/{id}")]
        public async Task<IActionResult> FlagVideo(string id)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);

            if (video == null)
            {
                return BadRequest($"Video With id {id} does not exist!");
            }

            video.Flags += 1;
            await _unitOfWork.Complete();

            return Ok($"Video Flags = {video.Flags}");
        }

        [HttpPost("View/{id}")]
        public async Task<IActionResult> ViewVideo(string id)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);

            if (video == null)
            {
                return BadRequest($"Video With id {id} does not exist!");
            }

            video.Views += 1;
            await _unitOfWork.Complete();

            return Ok($"Video views = {video.Views}");
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateVideo(UpdatingVideoDto model, string id)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(id);

            if (video == null)
            {
                return BadRequest($"Video With id {id} does not exist!");
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

            /*
             think with awad about update uri and videos name in cloud or not
            */

            await _unitOfWork.Complete();
            return Ok("Video Updated Sucessfully");
        }


    }
}
