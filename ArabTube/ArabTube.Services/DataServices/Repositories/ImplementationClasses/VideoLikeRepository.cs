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
    public class VideoLikeRepository : GenericRepository<VideoLike>, IVideoLikeRepository
    {
        public VideoLikeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckUserLikeVideoOrNotAsync (string userId , string videoId)
        {
            var videoLike = await _dbSet.FindAsync(userId, videoId);
            if (videoLike == null)
                return false;
            return true;
        }

        public async Task<bool> RemoveUserVideoLikeAsync (string userId , string videoId)
        {
            var videoLike = await _dbSet.FindAsync(userId, videoId);
            if (videoLike == null)
                return false;
            _dbSet.Remove(videoLike);
            return true;
        }
    }
}
