using ArabTube.Entities.DtoModels.VideoDTOs;
using ArabTube.Entities.Enums;
using ArabTube.Entities.Models;
using ArabTube.Entities.VideoModels;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using ArabTube.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static FFmpeg.NET.MetaData;

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

        // AutoMapped
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
                return BadRequest("Failed to Upload video");
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
            var likeVideo = await _videoService.LikeVideo(id);

            if (likeVideo == false)
            {
                return BadRequest($"Failed to like video With id {id}!");
            }
     
            var playlistId = await _playlistService.GetPlaylistId
                                          (PlaylistDefaultNames.PlaylistNames[0], true);
         
            var isVideoAdded = await _playlistVideoService.AddVideoToPlayListAsync(id, playlistId);
            if (!isVideoAdded)
            {
                return BadRequest($"Failed to add video With id {id} to the Liked Playlist!");
            }
            await _unitOfWork.Complete();

            return Ok($"User has Liked Video successfully!");
        }

        // dont forget to handle Save changes 
        [Authorize]
        [HttpPost("Dislike")]
        public async Task<IActionResult> DislikeVideo(string videoId)
        {
            var dislikeVideo = await _videoService.DislikeVideo(videoId);

            if (dislikeVideo == false)
            {
                return NotFound($"Failed to Dislike video With id {videoId}!");
            }

            // remove video from LikedPlaylist
            var playlistId = await _playlistService.GetPlaylistId
                                            (PlaylistDefaultNames.PlaylistNames[0], true);
         
            var result = await _playlistVideoService.RemoveVideoFromPlaylistAsync(videoId, playlistId);
            if (!result)
            {
                return BadRequest($"Failed to remove video With id {videoId} from Liked Playlist!");
            }

            await _unitOfWork.Complete();
            return Ok($"Video has Disliked successfully ");
        }

        // dont forget to handle Save changes 
        [Authorize]
        [HttpPost("Flag")]
        public async Task<IActionResult> FlagVideo(string id)
        {
            var video = await _videoService.FlagVideo(id);
            if (video == false)
            {
                return NotFound($"Video With id {id} does not exist!");
            }

            await _unitOfWork.Complete();

            return Ok("User Flaged video successfully");
        }

        [Authorize]
        [HttpPost("View")]
        public async Task<IActionResult> ViewVideo(string id)
        {
            var video = await _videoService.ViewVideo(id);
            if (video == false)
            {
                return NotFound($"Video With id {id} does not exist!");
            }

            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var isWatchedVideoAdded = await _watchedVideoService.AddWatchedVideoToHistoryAsync(user.Id, id);
                    if(!isWatchedVideoAdded)
                    {
                        return BadRequest("View Video Request Failed");
                    }
                }
            }

            await _unitOfWork.Complete();

            return Ok("User has watched video successfully");
        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateVideo(UpdatingVideoDto model, string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _videoService.UpdateVideo(model, id);
            if (!result)
            {
                return BadRequest("Failed To Update the Video");
            }

            await _unitOfWork.Complete();
            return Ok("Video Updated Sucessfully");
        }

        [Authorize]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteVideo(string id)
        {
            await _commentService.DeleteVideoCommentsAsync(id);
            await _videoService.DeleteAsync(id);

            await _unitOfWork.Complete();
            return Ok("Video Deleted Successfully");
        }
    }
}
  