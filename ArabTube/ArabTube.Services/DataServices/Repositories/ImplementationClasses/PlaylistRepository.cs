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
    public class PlaylistRepository : GenericRepository<Playlist>, IPlaylistRepository
    {
        public PlaylistRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Playlist>> GetPlaylistsAsync(string userId, bool includePrivate)
        {
            List<Playlist> playlists = new List<Playlist>(); 
            if (includePrivate)
            {
                playlists = await _dbSet.Where(p => p.UserId == userId).ToListAsync();
            }
            else
            {
                playlists = await _dbSet.Where(p => p.UserId == userId && !p.IsPrivate).ToListAsync();
            }
            return playlists;
        }

        public async Task<bool> DeletePlaylistAsync(string playlistId, string userId)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(p => p.UserId == userId && p.Id == playlistId);
            if (entity == null)
            {
                return false;
            }
            _dbSet.Remove(entity);
            return true;
        }

        public async Task<string> FindPlaylistByNameAsync(string title, bool IsDefult)
        {
            var playlist = await _dbSet.FirstOrDefaultAsync(p => p.Title == title && p.IsDefult == IsDefult);
            return playlist.Id;
        }
    }
}
