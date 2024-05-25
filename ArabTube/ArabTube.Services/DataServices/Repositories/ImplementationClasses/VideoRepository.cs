using ArabTube.Entities.Models;
using ArabTube.Services.DataServices.Data;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArabTube.Services.DataServices.Repositories.ImplementationClasses
{
    public class VideoRepository : GenericRepository<Video>, IVideoRepository
    {
        public VideoRepository(AppDbContext context) : base(context)
        {

        }
        public override async Task<IEnumerable<Video>> GetAllAsync()
        {
            var videos = await _dbSet.Include(v => v.AppUser).ToListAsync();
            return videos;
        }

        public override async Task<Video?> FindByIdAsync(string id)
        {
            var video = await _dbSet.Include(v => v.AppUser).FirstOrDefaultAsync(v => v.Id == id);
            return video;
        }

        public async Task<IEnumerable<Video>> SearchVideoAsync(string query)
        {
            var videos = await _dbSet.Where(v => v.Title.Contains(query)   || v.Description.Contains(query))
                                     .Include(v => v.AppUser).ToListAsync();
            return videos;
        }

        public async Task<IEnumerable<string>> SearchVideoTitlesAsync(string query)
        {
            var titles = await _dbSet.Where(v => v.Title.Contains(query) || v.Description.Contains(query)).Select(v => v.Title)
                                     .ToListAsync();
            return titles;
        }

        public async Task<IEnumerable<Video>> GetUserVideos(string userId)
        {
            var videos = await _dbSet.Where(v => v.UserId == userId).ToListAsync();
            return videos;
        }
    }
}
