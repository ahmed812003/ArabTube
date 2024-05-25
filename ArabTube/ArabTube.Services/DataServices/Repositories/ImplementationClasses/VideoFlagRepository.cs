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
    public class VideoFlagRepository : GenericRepository<VideoFlag>, IVideoFlagRepository
    {
        public VideoFlagRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckUserFlagVideoOrNotAsync(string userId, string videoId)
        {
            var videoLike = await _dbSet.FindAsync(userId, videoId);
            if (videoLike == null)
                return false;
            return true;
        }

        public async Task<bool> RemoveUserVideoFlagAsync(string userId, string videoId)
        {
            var videoLike = await _dbSet.FindAsync(userId, videoId);
            if (videoLike == null)
                return false;
            _dbSet.Remove(videoLike);
            return true;
        }
    }
}
