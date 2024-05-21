using ArabTube.Entities.Models;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using ArabTube.Services.Interfaces;

namespace ArabTube.Services.ImplementationClasses
{
    public class CommentService:ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void DeleteComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCommentAsync(string commentId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteVideoCommentsAsync(string videoId)
        {
             await _unitOfWork.Comment.DeleteVideoCommentsAsync(videoId);
        }

        public Task<IEnumerable<Comment>> GetCommentAsync(string commentId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Comment>> GetVideoCommentsAsync(string videoId)
        {
            throw new NotImplementedException();
        }
    }
}
