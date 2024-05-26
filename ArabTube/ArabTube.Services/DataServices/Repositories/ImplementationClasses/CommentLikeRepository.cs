using ArabTube.Entities.Models;
using ArabTube.Services.DataServices.Data;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.ImplementationClasses
{
    public class CommentLikeRepository : GenericRepository<CommentLike>, ICommentLikeRepository
    {
        public CommentLikeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckUserLikeCommentOrNotAsync(string commentId, string userId)
        {
            var commentLike = await _dbSet.FindAsync(commentId, userId);
            if (commentLike == null)
                return false;
            return true;
        }

        public async Task<bool> RemoveUserCommentLikeAsync(string commentId, string userId)
        {
            var commentLike = await _dbSet.FindAsync(commentId, userId);
            if (commentLike == null)
                return false;
            _dbSet.Remove(commentLike);
            return true;
        }
    }
}
