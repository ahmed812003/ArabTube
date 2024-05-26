using ArabTube.Entities.DtoModels.PlaylistDTOs;
using ArabTube.Entities.DtoModels.VideoDTOs;
using ArabTube.Entities.Models;
using ArabTube.Services.ControllersServices.PlaylistServices.Interfaces;
using ArabTube.Services.ControllersServices.PlaylistVideoServices.Interfaces;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly IPlaylistService _playlistService;
        private readonly IPlaylistVideoService _playlistVideoService;

        public PlaylistsController(UserManager<AppUser> userManager, IPlaylistService playlistService, IPlaylistVideoService playlistVideoService)
        {
            this._userManager = userManager;
            this._playlistService = playlistService;
            this._playlistVideoService = playlistVideoService;
        }

        [HttpGet("searchTitles")]
        public async Task<IActionResult> SearchPlaylistsTitles(string query)
        {
            var result = await _playlistService.SearchPlaylistsTitlesAsync(query);

            if (!result.IsSuccesed)
                return BadRequest(result.Message);
            
            return Ok(result.Titles);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchPlaylists(string query)
        {
            var result = await _playlistService.SearchPlaylistsAsync(query);

            if (!result.IsSuccesed)
                return BadRequest(result.Message);

            return Ok(result.Playlists);
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
                    var result = await _playlistService.GetMyPlaylistsAsync(user.Id);

                    if (!result.IsSuccesed)
                        return BadRequest(result.Message);
                    return Ok(result.Playlists);
                }
            }
            return Unauthorized();
        }

        [HttpGet("Playlist")]
        public async Task<IActionResult> GetPlaylists(string id)
        {
            var result = await _playlistService.GetPlaylistsAsync(id);

            if (!result.IsSuccesed)
                return BadRequest(result.Message);

            return Ok(result.Playlists);
        }

        [HttpGet("Video")]
        public async Task<IActionResult> GetPlaylistVideos(string id)
        {
            var result = await _playlistVideoService.GetPlaylistVideosAsync(id);

            if (!result.IsSuccesed)
                return BadRequest(result.Message);

            return Ok(result.Videos);
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
                    var result = await _playlistService.CreatePlaylistAsync(model, user.Id);
                    
                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }

                    return Ok("Playlist Created Succesfully");
                }  
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpPost("Save")]
        public async Task<IActionResult> SavePlaylist(string playlistId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _playlistService.SavePlaylistAsync(playlistId, user.Id);
                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }
                    return Ok("Playlist Saved!");
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
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _playlistVideoService.AddVideoToPlayListAsync(model.VideoId, model.PlaylistId , user.Id);

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
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdatePlaylistDto model)
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
                    var result = await _playlistService.UpdatePlaylistAsync(model , user.Id);

                    if (!result.IsSuccesed)
                        return BadRequest(result.Message);

                    return Ok("playlist Updated Successfully");
                }
            }
            return Unauthorized();        
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
                    var result = await _playlistService.DeletePlaylistAsync(id , user.Id);
                    
                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }

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

            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _playlistVideoService.RemoveVideoFromPlaylistAsync(model.VideoId, model.PlaylistId , user.Id);

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
