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
    public class CommentDislikeRepository : GenericRepository<CommentDislike>, IcommentDislikeRepository
    {
        public CommentDislikeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckUserDislikeCommentOrNotAsync(string commentId, string userId)
        {
            var commentDislike = await _dbSet.FindAsync(commentId, userId);
            if (commentDislike == null)
                return false;
            return true;
        }

        public async Task<bool> RemoveUserCommentDislikeAsync(string commentId, string userId)
        {
            var commentDislike = await _dbSet.FindAsync(commentId, userId);
            if (commentDislike == null)
                return false;
            _dbSet.Remove(commentDislike);
            return true;
        }
    }
}
