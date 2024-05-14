using ArabTube.Entities.DtoModels.PlaylistDTOs;
using ArabTube.Entities.Models;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static FFmpeg.NET.MetaData;

namespace ArabTube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public PlaylistsController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
        }

        [HttpGet("Get/{userId}")]
        public async Task<IActionResult> GetPlaylists(string userId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var playlists = await _unitOfWork.Playlist.GetPlaylistsAsync(userId, userId == user.Id);
                    var playlistsDto = playlists.Select(p => new GetPlaylistDto
                    {
                        Title = p.Title,
                        IsPrivate = p.IsPrivate
                    });

                    return Ok(playlistsDto);
                }
                else
                {
                    return Unauthorized();
                }
            }
            return Ok();
        }

        [HttpGet("GetVideos/{playlistId}")]
        public async Task<IActionResult> GetPlaylistVideos(string playlistId)
        {
            var playlistVideos = await _unitOfWork.PlaylistVideo.GetPlaylistVideosAsync(playlistId);
            if (playlistVideos != null)
            {
                var videos = playlistVideos.Select(pv => new PlaylistVideoDto
                {
                    VideoId = pv.VideoId,
                    Title = pv.Video.Title,
                    Views = pv.Video.Views,
                    CreatedTime =pv.Video.CreatedOn ,
                    Username = pv.Video.AppUser.UserName,
                    Thumbnail = pv.Video.Thumbnail,

                });
                return Ok(videos);
            }
            return Ok();
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreatePlaylistDto model)
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
                    var playlist = new Playlist
                    {
                        UserId = user.Id,
                        Title = model.Title,
                        Description = model.Description,
                        IsPrivate = model.IsPrivate
                    };

                    await _unitOfWork.Playlist.AddAsync(playlist);
                    await _unitOfWork.Complete();
                }
                else
                {
                    return Unauthorized();
                }
            }
            return Ok("Playlist Created Succesfully");
        }

        [Authorize]
        [HttpPost("AddVideo")]
        public async Task<IActionResult> AddVideoToPlaylist(AddPlaylistVideoDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _unitOfWork.PlaylistVideo.AddVideoToPlayListAsync(model.VideoID, model.PlaylistID);
            await _unitOfWork.Complete();
            return Ok("Video Added Succesfully");
        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdatePlaylistDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var playlist = await _unitOfWork.Playlist.FindByIdAsync(model.PlaylistId);
            if(playlist == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(model.Title))
                playlist.Title = model.Title;

            if (!string.IsNullOrEmpty(model.Description))
                playlist.Title = model.Description;
            await _unitOfWork.Complete();

            return Ok("playlist Updated Successfully");
        }
        
        [Authorize]
        [HttpDelete("Delete/{playlistId}")]
        public async Task<IActionResult> Delete(string playlistId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _unitOfWork.Playlist.DeletePlaylistAsync(playlistId , user.Id);
                    if (!result)
                        return Unauthorized();
                    await _unitOfWork.Complete();
                    return Ok("Playlist Deleted Successfully");
                }
            }
            return Ok();
        }

        [Authorize]
        [HttpDelete("RemoveVideo")]
        public async Task<IActionResult> RemoveideoFromPlaylist(RemovePlaylistVideoDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _unitOfWork.PlaylistVideo.DeleteVideosPlaylistAsync(model.PlaylistID);
            await _unitOfWork.PlaylistVideo.RemoveVideoFromPlayListAsync(model.VideoID, model.PlaylistID);
            await _unitOfWork.Complete();
            return Ok("Video Removed Succesfully");
        }

    }
}
