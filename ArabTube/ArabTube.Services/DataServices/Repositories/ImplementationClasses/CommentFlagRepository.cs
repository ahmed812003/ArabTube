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
    public class CommentFlagRepository : GenericRepository<CommentFlag>, ICommentFlagRepository
    {
        public CommentFlagRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckUserFlagCommentOrNotAsync(string commentId, string userId)
        {
            var commentFlag = await _dbSet.FindAsync(commentId, userId);
            if (commentFlag == null)
                return false;
            return true;
        }

        public async Task<bool> RemoveUserCommentFlagAsync(string commentId, string userId)
        {
            var commentFlag = await _dbSet.FindAsync(commentId, userId);
            if (commentFlag == null)
                return false;
            _dbSet.Remove(commentFlag);
            return true;
        }

    }
}
