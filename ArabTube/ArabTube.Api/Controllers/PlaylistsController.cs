using ArabTube.Entities.DtoModels.PlaylistDTOs;
using ArabTube.Entities.DtoModels.VideoDTOs;
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

        [HttpGet("searchTitles")]
        public async Task<IActionResult> SearchVideoTitles(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query Cannot Be Empty");

            var titles = await _unitOfWork.Playlist.SearchPlaylistTitlesAsync(query);

            if (!titles.Any())
                return NotFound("No Titles Found Matching The Search Query");

            return Ok(titles);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchVideos(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query Cannot Be Empty");

            var playlists = await _unitOfWork.Playlist.SearchPlaylistAsync(query);

            if (!playlists.Any())
                return NotFound("No Videos Found Matching The Search Query");

            var playlistsDto = playlists.Select(p => new GetPlaylistDto
            {
                Id = p.Id,
                Title = p.Title,
                IsPrivate = p.IsPrivate
            });

            return Ok(playlistsDto);
        }

        [Authorize]
        [HttpGet("MyPlaylist")]
        public async Task<IActionResult> GetMyPlaylists()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var playlists = await _unitOfWork.Playlist.GetPlaylistsAsync(user.Id, true);

                    if (!playlists.Any())
                        return NotFound("You Don't Have Any Playlist");

                    var playlistsDto = playlists.Select(p => new GetPlaylistDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        IsPrivate = p.IsPrivate
                    });
                    return Ok(playlistsDto);
                }
            }
            return Unauthorized();
        }

        [HttpGet("Playlist")]
        public async Task<IActionResult> GetPlaylists(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"No User With Id = {id}");
            }

            var playlists = await _unitOfWork.Playlist.GetPlaylistsAsync(id, false);
            
            if (!playlists.Any())
                return NotFound($"The User With Id = {id} Dosn't Has Any Playlist");
            
            var playlistsDto = playlists.Select(p => new GetPlaylistDto
            {
                Id = p.Id,
                Title = p.Title,
                IsPrivate = p.IsPrivate
            });

            return Ok(playlistsDto);
        }

        [HttpGet("Video")]
        public async Task<IActionResult> GetPlaylistVideos(string id)
        {
            var playlist = await _unitOfWork.Playlist.FindByIdAsync(id);
            if(playlist == null)
            {
                return NotFound($"No Playlist With Id = {id}");
            }

            var playlistVideos = await _unitOfWork.PlaylistVideo.GetPlaylistVideosAsync(id);

            if (!playlistVideos.Any())
                return NotFound("The Playlist With Id = {id} Dosn't Has Any Videos");

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
                    return Ok("Playlist Created Succesfully");
                }  
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpPost("AddVideo")]
        public async Task<IActionResult> AddVideoToPlaylist(AddPlaylistVideoDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var video = await _unitOfWork.Video.FindByIdAsync(model.VideoId);
            if(video == null)
            {
                return NotFound($"No Video With Id = {model.VideoId}");
            }

            var playlist = await _unitOfWork.Playlist.FindByIdAsync(model.PlaylistId);
            if(playlist == null)
            {
                return NotFound($"No Playlist With Id = {model.PlaylistId}");
            }

            var result = await _unitOfWork.PlaylistVideo.AddVideoToPlayListAsync(model.VideoId, model.PlaylistId);
            if (!result)
            {
                return Ok("The Video Is Already In The Playlist");
            }
                
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
                return NotFound($"No Playlist With Id = {model.PlaylistId}");
            }
            if (playlist.IsDefult)
            {
                return BadRequest("Can't Delete Defult Playlists");
            }

            if (!string.IsNullOrEmpty(model.Title))
                playlist.Title = model.Title;

            if (!string.IsNullOrEmpty(model.Description))
                playlist.Title = model.Description;
            await _unitOfWork.Complete();

            return Ok("playlist Updated Successfully");
        }
        
        [Authorize]
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var playlist = await _unitOfWork.Playlist.FindByIdAsync(id);
                    if(playlist == null)
                    {
                        return NotFound($"No Playlist With Id = {id}");
                    }

                    if (playlist.IsDefult)
                    {
                        return BadRequest("Can't Delete Defult Playlists");
                    }

                    await _unitOfWork.PlaylistVideo.DeletePlaylistVideosAsync(id);
                    await _unitOfWork.Playlist.DeletePlaylistAsync(id);
                    await _unitOfWork.Complete();
                    return Ok("Playlist Deleted Successfully");
                }
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpDelete("RemoveVideo")]
        public async Task<IActionResult> RemoveVideoFromPlaylist(RemovePlaylistVideoDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var video = await _unitOfWork.Video.FindByIdAsync(model.VideoId);
            if (video == null)
            {
                return NotFound($"No Video With Id = {model.VideoId}");
            }

            var playlist = await _unitOfWork.Playlist.FindByIdAsync(model.PlaylistId);
            if (playlist == null)
            {
                return NotFound($"No Playlist With Id = {model.PlaylistId}");
            }

            var result = await _unitOfWork.PlaylistVideo.RemoveVideoFromPlayListAsync(model.VideoId, model.PlaylistId);
            if (!result)
            {
                return NotFound("The Video Is Already Not In The Playlist");
            }

            await _unitOfWork.Complete();
            return Ok("Video Removed Succesfully");
        }


    }
}
