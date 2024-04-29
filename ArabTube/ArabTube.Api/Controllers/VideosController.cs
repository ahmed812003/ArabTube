using ArabTube.Entities.DtoModels.VideoDTOs;
using ArabTube.Entities.VideoModels;
using ArabTube.Services.CloudServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using ArabTube.Services.VideoServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

        public VideosController(IVideoService videoService, IUnitOfWork unitOfWork, ICloudService cloudService)
        {
            _videoService = videoService;
            _unitOfWork = unitOfWork;
            _cloudService = cloudService;
        }

        [HttpPost("Uploda")]
        public async Task<IActionResult> UploadVideo([FromForm]UploadingVideoDto model)
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

            var processingVideo = new ProcessingVideo
            {
                Username = userName,
                Video = model.Video,
                Title = model.Title
            };

            var videoQualities = await _videoService.ProcessVideoAsync(processingVideo);

            await _cloudService.UploadToCloudAsync(videoQualities);

            return Ok("Video Uploaded Sucessfully");
        }
    }
}
