using ArabTube.Entities.CommentModels;
using ArabTube.Entities.DtoModels.CommentDTOs;
using ArabTube.Entities.GenericModels;

namespace ArabTube.Services.ControllersServices.CommentServices.Interfaces
{
    public interface ICommentService
    {
        Task<ProcessResult> DeleteVideoCommentsAsync(string videoId , string userId);
        Task<GetCommentResult> GetCommentAsync(string commentId);
        Task<GetVideoCommentsResult> GetVideoCommentsAsync(string videoId);
        Task<ProcessResult> IsUserLikeCommentAsync(string commentId, string userId);
        Task<ProcessResult> IsUserDislikeCommentAsync(string commentId, string userId);
        Task<ProcessResult> IsUserFlagCommentAsync(string commentId, string userId);
        Task<ProcessResult> AddCommentAsync(AddCommentDto model, string userId, string channelTitle);
        Task<ProcessResult> LikeCommentAsync(string commentId, string userId, string channelTitle);
        Task<ProcessResult> DislikeCommentAsync(string commentId, string userId, string channelTitle);
        Task<ProcessResult> FlagCommentAsync(string commentId, string userId);
        Task<ProcessResult> UpdateCommentAsync(UpdateCommentDto model, string userId);
        Task<ProcessResult> DeleteCommentAsync(string commentId, string userId);
    }
}
