using ArabTube.Entities.DtoModels.CommentDTOs;
using ArabTube.Entities.DtoModels.PlaylistDTOs;
using ArabTube.Entities.Models;
using ArabTube.Services.ControllersServices;
using ArabTube.Services.ControllersServices.CommentServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArabTube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICommentService _commentService;

        public CommentsController(UserManager<AppUser> userManager, ICommentService commentService)
        {
            this._userManager = userManager;
            this._commentService = commentService;
        }

        [HttpGet("Comments")]
        public async Task<IActionResult> GetVideoComments(string id)
        {
            var result = await _commentService.GetVideoCommentsAsync(id);
            
            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Comments);
        }

        [HttpGet("Comment")]
        public async Task<IActionResult> GetComment(string id)
        {
            var result = await _commentService.GetCommentAsync(id);

            if (!result.IsSuccesed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Comment);
        }

        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> AddComment (AddCommentDto model)
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
                    var result = await _commentService.AddCommentAsync(model, user.Id);
                    
                    if (!result.IsSuccesed)
                        return BadRequest(result.Message);

                    return Ok("Comment Added Succesfully");
                }
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpPost("Like")]
        public async Task<IActionResult> LikeComment(string id)
        {
            var result = await _commentService.LikeCommentAsync(id);
            if (!result.IsSuccesed)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [Authorize]
        [HttpPost("Dislike")]
        public async Task<IActionResult> DislikeComment(string id)
        {
            var result = await _commentService.DislikeCommentAsync(id);
            if (!result.IsSuccesed)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateComment(UpdateCommentDto model)
        {
            var result = await _commentService.UpdateCommentAsync(model);
            if (!result.IsSuccesed)
                return BadRequest(result.Message);
            return Ok("Comment Updated Successfully");
        }

        [Authorize]
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _commentService.DeleteCommentAsync(id);
            if (!result.IsSuccesed)
                return BadRequest(result.IsSuccesed);
            return Ok("Comment Deleted Successfully");
        }

    }
}
