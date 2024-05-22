using ArabTube.Entities.CommentModels;
using ArabTube.Entities.DtoModels.CommentDTOs;
using ArabTube.Entities.GenericModels;
using ArabTube.Entities.Models;
using ArabTube.Services.ControllersServices.CommentServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;

namespace ArabTube.Services.ControllersServices.CommentServices.ImplementationClasses
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetVideoCommentsResult> GetVideoCommentsAsync(string videoId)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(videoId);
            if (video == null)
            {
                return new GetVideoCommentsResult { Message = $"No video with id = {videoId} Exist!"};
            }

            var comments = await _unitOfWork.Comment.GetVideoCommentsAsync(videoId);

            if (!comments.Any())
                return new GetVideoCommentsResult { Message = $"The Video With Id = {videoId} Dosn't Has Any Comments" };

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

            return new GetVideoCommentsResult
            {
                IsSuccesed = true,
                Comments = commentsDto
            };
        }

        public async Task<GetCommentResult> GetCommentAsync(string commentId)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(commentId);
            if (comment == null || comment.Id != comment.ParentCommentId)
            {
                return new GetCommentResult {Message = "Invalid Comment Id " };
            }

            comment = await _unitOfWork.Comment.GetCommentAsync(commentId);

            var commentDto = new GetCommentDto
            {
                commentId = comment.Id,
                Username = comment.AppUser.UserName,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                IsUpdated = comment.IsUpdated,
                Mention = comment.Mention,
                Likes = comment.Likes,
                DisLike = comment.DisLikes,
                childrens = comment.Childrens.Where(cc => cc.Id != comment.Id).Select(cc => new GetCommentDto
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
            };

            return new GetCommentResult
            {
                IsSuccesed = true,
                Comment = commentDto
            };
        }

        public async Task<ProcessResult> AddCommentAsync(AddCommentDto model , string userId)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(model.VideoId);
            if (video == null)
            {
                return new ProcessResult { Message = $"No Video Wiht Id = {model.VideoId}" };
            }

            var parentComment = await _unitOfWork.Comment.FindByIdAsync(model.ParentCommentId);
            if (!string.IsNullOrEmpty(model.ParentCommentId) && parentComment == null)
            {
                return new ProcessResult { Message = $"No Comment Wiht Id = {model.ParentCommentId}" };
            }

            var comment = new Comment
            {
                Content = model.Content,
                CreatedOn = DateTime.Now,
                Mention = model.Mention,
                UserId = userId,
                VideoId = model.VideoId
            };

            if (string.IsNullOrEmpty(model.ParentCommentId))
                comment.ParentCommentId = comment.Id;
            else
                comment.ParentCommentId = parentComment.ParentCommentId;

            await _unitOfWork.Comment.AddAsync(comment);
            await _unitOfWork.Complete();

            return new ProcessResult { IsSuccesed = true};
        }

        public async Task<ProcessResult> LikeCommentAsync(string commentId)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(commentId);

            if (comment == null)
            {
                return new ProcessResult { Message = $"Comment With id {commentId} does not exist!" };
            }

            comment.Likes += 1;
            await _unitOfWork.Complete();

            return new ProcessResult
            {
                IsSuccesed = true,
                Message = $"Comment Likes = {comment.Likes}"
            };
        }

        public async Task<ProcessResult> DislikeCommentAsync(string commentId)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(commentId);

            if (comment == null)
            {
                return new ProcessResult { Message = $"Comment With id {commentId} does not exist!" };
            }

            comment.DisLikes += 1;
            await _unitOfWork.Complete();

            return new ProcessResult
            {
                IsSuccesed = true,
                Message = $"Comment Dislikes = {comment.DisLikes}"
            };
        }

        public async Task<ProcessResult> UpdateCommentAsync(UpdateCommentDto model)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(model.CommentId);
            if (comment == null)
            {
                return new ProcessResult { Message = $"Comment With id {model.CommentId} does not exist!" };
            }
            comment.Content = model.Content;
            comment.IsUpdated = true;
            await _unitOfWork.Complete();
            return new ProcessResult
            {
                IsSuccesed = true
            };
        }

        public async Task<ProcessResult> DeleteCommentAsync(string commentId)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(commentId);
            if (comment == null)
            {
                return new ProcessResult { Message = $"Comment With id {commentId} does not exist!" };
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
            return new ProcessResult { IsSuccesed = true};
        }

        public async Task<ProcessResult> DeleteVideoCommentsAsync(string videoId)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(videoId);
            if(video == null)
            {
                return new ProcessResult { Message = $"No video Exists wiht id {videoId}" };
            }

            await _unitOfWork.Comment.DeleteVideoCommentsAsync(videoId);
            await _unitOfWork.Complete();
            return new ProcessResult { IsSuccesed = true };
        }
    }
}
