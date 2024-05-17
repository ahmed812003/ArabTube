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
    public class VideoRepository : GenericRepository<Video>, IVideoRepository
    {
        public VideoRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Video>> SearchVideoAsync(string query)
        {
            var videos = await _dbSet.Where(v => v.Title.Contains(query) || v.Description.Contains(query))
                                     .ToListAsync();
            return videos;
        }

        public async Task<IEnumerable<string>> SearchVideoTitlesAsync(string query)
        {
            var titles = await _dbSet.Where(v => v.Title.Contains(query)).Select(v => v.Title)
                                     .ToListAsync();
            return titles;
        }
    }
}
