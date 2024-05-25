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
    public class VideoDislikeRepository : GenericRepository<VideoDislike>, IVideoDislikeRepository
    {
        public VideoDislikeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckUserDislikeVideoOrNotAsync(string userId,string videoId)
        {
            var videoDislike = await _dbSet.FindAsync(userId, videoId);
            if (videoDislike == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> RemoveUserVideoDislikeAsync(string userId , string videoId)
        {
            var videoDislike =await _dbSet.FindAsync(userId,videoId);
            if (videoDislike == null)
                return false;
            _dbSet.Remove(videoDislike);
            return true;
        }
    }
}
