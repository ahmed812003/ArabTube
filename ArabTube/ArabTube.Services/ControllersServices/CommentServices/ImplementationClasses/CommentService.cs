using ArabTube.Entities.CommentModels;
using ArabTube.Entities.DtoModels.CommentDTOs;
using ArabTube.Entities.DtoModels.VideoDTOs;
using ArabTube.Entities.GenericModels;
using ArabTube.Entities.Models;
using ArabTube.Entities.VideoModels;
using ArabTube.Services.ControllersServices.CommentServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.Design;
using System.Xml.Linq;
using static FFmpeg.NET.MetaData;

namespace ArabTube.Services.ControllersServices.CommentServices.ImplementationClasses
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
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

            var commentsDto = _mapper.Map<IEnumerable<GetCommentDto>>(comments)
                                   .OrderBy(c => c.CreatedOn);

            return new GetVideoCommentsResult
            {
                IsSuccesed = true,
                Comments = commentsDto
            };
        }

        public async Task<GetCommentResult> GetCommentAsync(string commentId)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(commentId);
            if (comment == null)
            {
                return new GetCommentResult {Message = "Invalid Comment Id " };
            }
            if(comment.Id == comment.ParentCommentId)
                comment = await _unitOfWork.Comment.GetParentCommentAsync(commentId);
            else
                comment = await _unitOfWork.Comment.GetCommentAsync(commentId);

            var commentDto = _mapper.Map<GetCommentDto>(comment);
                               
            return new GetCommentResult
            {
                IsSuccesed = true,
                Comment = commentDto
            };
        }

        public async Task<GetFlagedCommentsResult> GetFlagedCommentsAsync()
        {
            var flagedComments = await _unitOfWork.FlagedComment.GetAllAsync();

            if (flagedComments == null || !flagedComments.Any())
            {
                return new GetFlagedCommentsResult { Message = "No Comments Found " };
            }

            var comments = new List<GetCommentDto>();
            foreach (var flagedComment in flagedComments)
            {
                var comment = await _unitOfWork.Comment.GetCommentAsync(flagedComment.CommentId);

                if (comment == null)
                {
                    await _unitOfWork.FlagedVideo.RemoveAsync(flagedComment.CommentId);
                }
                else
                {
                    var viewComment = _mapper.Map<GetCommentDto>(comment);
                    comments.Add(viewComment);
                }
            }
            await _unitOfWork.Complete();
            return new GetFlagedCommentsResult
            {
                IsSuccesed = true,
                getCommentDtos = comments
            };
        }

        public async Task<ProcessResult> IsUserLikeCommentAsync(string commentId, string userId)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(commentId);
            if(comment == null)
            {
                return new ProcessResult { Message = $"No comment exists wiht id = {commentId}" };
            }
            var isUserLikeComment = await _unitOfWork.CommentLike.CheckUserLikeCommentOrNotAsync(commentId, userId);
            return new ProcessResult
            {
                IsSuccesed = true,
                Message = isUserLikeComment ? "Yes" : "No"
            };
        }

        public async Task<ProcessResult> IsUserDislikeCommentAsync(string commentId, string userId)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(commentId);
            if (comment == null)
            {
                return new ProcessResult { Message = $"No comment exists wiht id = {commentId}" };
            }
            var isUserDislikeComment = await _unitOfWork.CommentDislike.CheckUserDislikeCommentOrNotAsync(commentId, userId);
            return new ProcessResult
            {
                IsSuccesed = true,
                Message = isUserDislikeComment ? "Yes" : "No"
            };
        }

        public async Task<ProcessResult> IsUserFlagCommentAsync(string commentId, string userId)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(commentId);
            if (comment == null)
            {
                return new ProcessResult { Message = $"No comment exists wiht id = {commentId}" };
            }
            var isUserFlagComment = await _unitOfWork.CommentFlag.CheckUserFlagCommentOrNotAsync(commentId, userId);
            return new ProcessResult
            {
                IsSuccesed = true,
                Message = isUserFlagComment ? "Yes" : "No"
            };
        }

        public async Task<ProcessResult> AddCommentAsync(AddCommentDto model , string userId , string channelTitle)
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
            var comment = _mapper.Map<Comment>(model);
            comment.UserId = userId;
            
            if (string.IsNullOrEmpty(model.ParentCommentId))
            {
                comment.ParentCommentId = comment.Id;
                if(userId != video.UserId)
                {
                    var notification = new Notification
                    {
                        Message = $"{channelTitle} comment in your video",
                        UserId = video.UserId,
                        SenderId = userId,
                        VideoId = video.Id,
                        Category = "Comment",
                        CommentId = comment.Id
                    };
                    await _unitOfWork.Notification.AddAsync(notification);
                }
            } 
            else
            {
                comment.ParentCommentId = parentComment.ParentCommentId;
                if(userId != parentComment.UserId)
                {
                    var notification = new Notification
                    {
                        Message = $"{channelTitle} reply to your comment",
                        UserId = parentComment.UserId,
                        SenderId = userId,
                        VideoId = video.Id,
                        Category = "Comment",
                        CommentId = comment.Id
                    };
                    await _unitOfWork.Notification.AddAsync(notification);
                }
            }
            await _unitOfWork.Comment.AddAsync(comment);
            await _unitOfWork.Complete();

            return new ProcessResult { IsSuccesed = true};
        }

        public async Task<ProcessResult> LikeCommentAsync(string commentId , string userId , string channelTitle)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(commentId);

            if (comment == null)
            {
                return new ProcessResult { Message = $"Comment With id {commentId} does not exist!" };
            }

            var isUserLikeComment = await _unitOfWork.CommentLike.CheckUserLikeCommentOrNotAsync(commentId , userId);
            if (isUserLikeComment)
            {   
                var isRemovedLike = await _unitOfWork.CommentLike.RemoveUserCommentLikeAsync(commentId , userId);
                if(!isRemovedLike)
                {
                    return new ProcessResult { Message = $"Cannot remove like comment with id = {commentId}" };
                }
                comment.Likes -= 1;
            }
            else
            {
                var commentLike = new CommentLike
                {
                    UserId = userId,
                    CommentId = commentId,
                };
                var isLikeAdded = await _unitOfWork.CommentLike.AddAsync(commentLike);
                if (!isLikeAdded)
                {
                    return new ProcessResult { Message = $"Cannot add like comment with id = {commentId}" };
                }
                comment.Likes += 1;
                var notification = new Notification
                {
                    Message = $"{channelTitle} like your comment",
                    UserId = comment.UserId,
                    SenderId = userId,
                    VideoId = string.Empty,
                    Category = "Comment",
                    CommentId = comment.Id
                };
                await _unitOfWork.Notification.AddAsync(notification);
            }

            await _unitOfWork.Complete();

            return new ProcessResult
            {
                IsSuccesed = true,
                Message = $"Comment Likes = {comment.Likes}"
            };
        }

        public async Task<ProcessResult> DislikeCommentAsync(string commentId, string userId, string channelTitle)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(commentId);

            if (comment == null)
            {
                return new ProcessResult { Message = $"Comment With id {commentId} does not exist!" };
            }

            var isUserDislikeComment = await _unitOfWork.CommentDislike.CheckUserDislikeCommentOrNotAsync(commentId, userId);
            if (isUserDislikeComment)
            {
                var isRemovedDislike = await _unitOfWork.CommentDislike.RemoveUserCommentDislikeAsync(commentId, userId);
                if (!isRemovedDislike)
                {
                    return new ProcessResult { Message = $"Cannot remove Dislike comment with id = {commentId}" };
                }
                comment.DisLikes -= 1;
            }
            else
            {
                var commentDislike = new CommentDislike
                {
                    UserId = userId,
                    CommentId = commentId,
                };
                var isDislikeAdded = await _unitOfWork.CommentDislike.AddAsync(commentDislike);
                if (!isDislikeAdded)
                {
                    return new ProcessResult { Message = $"Cannot add Dislike comment with id = {commentId}" };
                }
                comment.DisLikes += 1;
                var notification = new Notification
                {
                    Message = $"{channelTitle} Dislike your comment",
                    UserId = comment.UserId,
                    SenderId = userId,
                    VideoId = string.Empty,
                    Category = "Comment",
                    CommentId = comment.Id
                };
                await _unitOfWork.Notification.AddAsync(notification);
            }

            await _unitOfWork.Complete();

            return new ProcessResult
            {
                IsSuccesed = true,
                Message = $"Comment Dislikes = {comment.DisLikes}"
            };
        }

        public async Task<ProcessResult> FlagCommentAsync(string commentId, string userId)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(commentId);

            if (comment == null)
            {
                return new ProcessResult { Message = $"Comment With id {commentId} does not exist!" };
            }
            bool addToFlagComments;
            var isUserFlagComment = await _unitOfWork.CommentFlag.CheckUserFlagCommentOrNotAsync(commentId, userId);
            if (isUserFlagComment)
            {
                var isRemovedFlag = await _unitOfWork.CommentFlag.RemoveUserCommentFlagAsync(commentId, userId);
                if (!isRemovedFlag)
                {
                    return new ProcessResult { Message = $"Cannot remove flag comment with id = {commentId}" };
                }
                comment.Flags -= 1;
                addToFlagComments = (comment.Flags > 0);
            }
            else
            {
                var commentFlag = new CommentFlag
                {
                    UserId = userId,
                    CommentId = commentId,
                };
                var isFlagAdded = await _unitOfWork.CommentFlag.AddAsync(commentFlag);
                if (!isFlagAdded)
                {
                    return new ProcessResult { Message = $"Cannot add flag comment with id = {commentId}" };
                }
                comment.Flags += 1;
                addToFlagComments = true;
            }

            if (addToFlagComments)
            {
                var flagedComment = new FlagedComment
                {
                    VideoId = comment.VideoId,
                    CommentId = comment.Id
                };

                var isCommentAdded = await _unitOfWork.FlagedComment.AddAsync(flagedComment);
                if (!isCommentAdded)
                {
                    return new ProcessResult { Message = $"Failed to add video to flaged videos table!" };
                }
            }
            else
            {
                var isCommentRemoved = await _unitOfWork.FlagedVideo.RemoveAsync(comment.Id);
                if (!isCommentRemoved)
                {
                    return new ProcessResult { Message = $"Failed to remove video from flaged videos table!" };
                }
            }


            await _unitOfWork.Complete();

            return new ProcessResult
            {
                IsSuccesed = true,
                Message = $"Comment Flags = {comment.Flags}"
            };
        }

        public async Task<ProcessResult> UpdateCommentAsync(UpdateCommentDto model , string userId)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(model.CommentId);
            if (comment == null)
            {
                return new ProcessResult { Message = $"Comment With id {model.CommentId} does not exist!" };
            }

            if(comment.UserId != userId)
            {
                return new ProcessResult { Message = $"Unauthorized to update this comment" };
            }

            comment.Content = model.Content;
            comment.IsUpdated = true;
            await _unitOfWork.Complete();
            return new ProcessResult
            {
                IsSuccesed = true
            };
        }

        public async Task<ProcessResult> DeleteCommentAsync(string commentId , AppUser user)
        {
            var comment = await _unitOfWork.Comment.FindByIdAsync(commentId);
            if (comment == null)
            {
                return new ProcessResult { Message = $"Comment With id {commentId} does not exist!" };
            }
            var roles = await _userManager.GetRolesAsync(user);
            var admin = roles.Any(r => r == "Admin");
            if (comment.UserId != user.Id && !admin)
            {
                return new ProcessResult { Message = $"Unauthorized to delete this comment" };
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

        public async Task<ProcessResult> DeleteVideoCommentsAsync(string videoId , AppUser user)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(videoId);
            if(video == null)
            {
                return new ProcessResult { Message = $"No video Exists wiht id {videoId}" };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var admin = roles.Any(r => r == "Admin");

            if (video.UserId != user.Id && !admin)
            {
                return new ProcessResult { Message = $"Unauthorized to delete comments of this video" };
            }

            await _unitOfWork.Comment.DeleteVideoCommentsAsync(videoId);
            await _unitOfWork.Complete();
            return new ProcessResult { IsSuccesed = true };
        }

    }
}
