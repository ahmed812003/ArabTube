using ArabTube.Entities.Models;
using ArabTube.Services.DataServices.Data;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.ImplementationClasses
{
    public class PlaylistVideoRepository : GenericRepository<PlaylistVideo>, IPlaylistVideoRepository
    {
        public PlaylistVideoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PlaylistVideo>?> GetPlaylistVideosAsync(string playlistId)
        {
            var playlistVideos = await _dbSet.Where(p => p.PlaylistId == playlistId).Include(p => p.Video)
                .ThenInclude(v => v.AppUser).ToListAsync();
            return playlistVideos;
        }

        public async Task AddVideoToPlayListAsync(string videoId, string playlistId)
        {
            var entity = await _dbSet.FindAsync(playlistId , videoId);

            if(entity == null)
            {
                var newPlaylistVideo = new PlaylistVideo
                {
                    VideoId = videoId,
                    PlaylistId = playlistId,
                };
                await _dbSet.AddAsync(newPlaylistVideo);
            }
        }

        public async Task RemoveVideoFromPlayListAsync(string videoId, string playlistId)
        {
            var entity = await _dbSet.FindAsync(playlistId , videoId);

            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
    
        public async Task DeleteVideosPlaylistAsync(string playlistId)
        {
            var playlistVideos = await _dbSet.Where(pv => pv.PlaylistId == playlistId).ToListAsync();
            _dbSet.RemoveRange(playlistVideos);
        }
    }
}
