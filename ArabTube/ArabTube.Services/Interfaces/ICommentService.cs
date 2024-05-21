using ArabTube.Entities.Models;

namespace ArabTube.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetVideoCommentsAsync(string videoId);
        Task<IEnumerable<Comment>> GetCommentAsync(string commentId);
        Task DeleteCommentAsync(string commentId);
        Task DeleteVideoCommentsAsync(string videoId);
        void DeleteComment(Comment comment);
    }
}
