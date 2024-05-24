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
        Task<ProcessResult> AddCommentAsync(AddCommentDto model, string userId);
        Task<ProcessResult> LikeCommentAsync(string commentId);
        Task<ProcessResult> DislikeCommentAsync(string commentId);
        Task<ProcessResult> UpdateCommentAsync(UpdateCommentDto model, string userId);
        Task<ProcessResult> DeleteCommentAsync(string commentId, string userId);
    }
}
