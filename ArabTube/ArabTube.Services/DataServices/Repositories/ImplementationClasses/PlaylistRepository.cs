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

        public async Task<IEnumerable<Playlist>> SearchPlaylistAsync(string query)
        {
            var playlists = await _dbSet.Where(p => (p.Title.Contains(query) || p.Description.Contains(query)) && !p.IsPrivate)
                                     .ToListAsync();
            return playlists;
        }

        public async Task<IEnumerable<string>> SearchPlaylistTitlesAsync(string query)
        {
            var titles = await _dbSet.Where(p => p.Title.Contains(query) && !p.IsPrivate).Select(p => p.Title)
                                     .ToListAsync();
            return titles;
        }

        public async Task<bool> CheckDefultPlaylistsAsync(string userId)
        {
            var playlists = await _dbSet.FirstOrDefaultAsync(p => p.UserId == userId && p.IsDefult);
            if(playlists == null)
            {
                return false;
            }
            return true;
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

        public async Task<bool> DeletePlaylistAsync(string playlistId)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(p => p.Id == playlistId);
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
