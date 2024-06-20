using ArabTube.Entities.Models;
using ArabTube.Services.DataServices.Data;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.ImplementationClasses
{
    public class FlagedCommentRepository : GenericRepository<FlagedComment>, IFlagedCommentRepository
    {
        public FlagedCommentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> RemoveAsync(string commentId)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(fc => fc.CommentId == commentId);

            if (entity == null)
            {
                return false;
            }

            _dbSet.Remove(entity);
            return true;
        }

        public async Task<bool> RemoveRangeAsync(string videoId)
        {
            var entites = await _dbSet.Where(fc => fc.VideoId == videoId).ToListAsync();
            if (entites.Any())
            {
                _dbSet.RemoveRange(entites);
            }
            return true;
        }
    }
}
