using ArabTube.Entities.Models;
using ArabTube.Services.DataServices.Data;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArabTube.Services.DataServices.Repositories.ImplementationClasses
{
    public class WatchedVideoRepository : GenericRepository<WatchedVideo>, IWatchedVideoRepository
    {
        public WatchedVideoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<WatchedVideo>?> GetWatchedVideosAsync(string userId)
        {
            var watchedVideos = await _dbSet.Where(wv => wv.UserId == userId).Include(wv => wv.Video)
                .ThenInclude(v => v.AppUser)
                .OrderByDescending(wv => wv.WatchedTime).ToListAsync();
            return watchedVideos;
        }

        public async Task<bool> AddWatchedVideoToHistoryAsync(string userId, string videoId)
        {
            var entity = await _dbSet.FindAsync(userId,videoId);
            if (entity == null)
            {
                var newWatchedVideo = new WatchedVideo
                {
                    UserId = userId,
                    VideoId = videoId
                };
                await _dbSet.AddAsync(newWatchedVideo);
            }
            else
            {
                entity.WatchedTime = DateTime.Now;
            }
            return true;
        }

        public async Task<bool> DeleteWatchedVideoAsync(string videoId , string userId)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(wv => wv.VideoId == videoId && wv.UserId == userId);
            if(entity != null)
            {
                _dbSet.Remove(entity);
            }
            return true;
        }

        
    }
}
