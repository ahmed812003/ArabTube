using ArabTube.Entities.Models;
using System.ComponentModel.Design;
using static FFmpeg.NET.MetaData;


namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetVideoCommentsAsync(string videoId);
        Task<IEnumerable<Comment>> GetCommentAsync(string commentId);
        Task DeleteCommentAsync(string commentId);
        Task DeleteVideoCommentsAsync(string videoId);
        void DeleteComment(Comment comment);
    }
}
