using ArabTube.Entities.DtoModels.CommentDTOs;
using ArabTube.Entities.DtoModels.PlaylistDTOs;
using ArabTube.Entities.Models;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArabTube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public CommentsController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
        }

        [HttpGet("Comments/{videoId}")]
        public async Task<IActionResult> GetVideoComments(string videoId)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(videoId);
            if(video == null)
            {
                return NotFound();
            }

            var comments = await _unitOfWork.Comment.GetVideoCommentsAsync(videoId);
            var commentsDto = comments.Select(c => new GetCommentDto
            {
                commentId = c.Id,
                Username = c.AppUser.UserName,
                Content = c.Content,
                CreatedOn = c.CreatedOn,
                IsUpdated = c.IsUpdated,
                Mention = c.Mention,
                Likes = c.Likes,
                DisLike = c.DisLikes,
                childrens = c.Childrens.Where(cc => cc.Id != c.Id).Select(cc => new GetCommentDto
                {
                    commentId = cc.Id,
                    Username = cc.AppUser.UserName,
                    Content = cc.Content,
                    CreatedOn = cc.CreatedOn,
                    IsUpdated = cc.IsUpdated,
                    Mention = cc.Mention,
                    Likes = cc.Likes,
                    DisLike = cc.DisLikes,
                }).OrderBy(cc => cc.CreatedOn).ToList()
            }).OrderBy(c => c.CreatedOn);

            return Ok(commentsDto);
        }

        [HttpGet("Comment/{commentId}")]
        public async Task<IActionResult> GetComment(string commentId)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(commentId);
            if(comment == null || comment.Id != comment.ParentCommentId)
            {
                return NotFound();
            }

            var comments = await _unitOfWork.Comment.GetCommentAsync(commentId);
            
            var commentsDto = comments.Select(c => new GetCommentDto
            {
                commentId = c.Id,
                Username = c.AppUser.UserName,
                Content = c.Content,
                CreatedOn = c.CreatedOn,
                IsUpdated = c.IsUpdated,
                Mention = c.Mention,
                Likes = c.Likes,
                DisLike = c.DisLikes,
                childrens = c.Childrens.Where(cc => cc.Id != c.Id).Select(cc => new GetCommentDto
                {
                    commentId = cc.Id,
                    Username = cc.AppUser.UserName,
                    Content = cc.Content,
                    CreatedOn = cc.CreatedOn,
                    IsUpdated = cc.IsUpdated,
                    Mention = cc.Mention,
                    Likes = cc.Likes,
                    DisLike = cc.DisLikes
                }).OrderBy(cc => cc.CreatedOn).ToList()
            }).OrderBy(c => c.CreatedOn);

            return Ok(commentsDto);
        }

        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> AddComment (AddCommentDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parentComment = await _unitOfWork.Comment.FindByIdAsync(model.ParentCommentId);
            if(parentComment == null)
            {
                return NotFound();
            }

            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var comment = new Comment
                    {
                        Content = model.Content,
                        CreatedOn = DateTime.Now,
                        Mention = model.Mention,
                        UserId = user.Id,
                        VideoId = model.VideoId
                    };
                    if (string.IsNullOrEmpty(model.ParentCommentId))
                        comment.ParentCommentId = comment.Id;
                    else
                        comment.ParentCommentId = parentComment.ParentCommentId;

                    await _unitOfWork.Comment.AddAsync(comment);
                    await _unitOfWork.Complete();
                    return Ok("Comment Added Succesfully");
                }
                else
                {
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpPost("Like/{id}")]
        public async Task<IActionResult> LikeComment(string id)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(id);

            if (comment == null)
            {
                return BadRequest($"Comment With id {id} does not exist!");
            }

            comment.Likes += 1;
            await _unitOfWork.Complete();

            return Ok($"Comment Likes = {comment.Likes}");
        }

        [Authorize]
        [HttpPost("Dislike/{id}")]
        public async Task<IActionResult> DislikeComment(string id)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(id);

            if (comment == null)
            {
                return BadRequest($"Comment With id {id} does not exist!");
            }

            comment.DisLikes += 1;
            await _unitOfWork.Complete();

            return Ok($"Comment Dislikes = {comment.DisLikes}");
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateComment(UpdateCommentDto model)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(model.CommentId);
            if(comment == null)
            {
                return NotFound();
            }
            comment.Content = model.Content;
            comment.IsUpdated = true;
            await _unitOfWork.Complete();
            return Ok("Comment Updated Successfully");
        }

        [Authorize]
        [HttpDelete("Delete/{commentId}")]
        public async Task<IActionResult> Delete(string commentId)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            if (comment.ParentCommentId == commentId)
            {
                await _unitOfWork.Comment.DeleteCommentAsync(comment.Id);
            }
            else
            {
                _unitOfWork.Comment.DeleteComment(comment);
            }
            await _unitOfWork.Complete();
            return Ok("Comment Deleted Successfully");
        }


    }
}
