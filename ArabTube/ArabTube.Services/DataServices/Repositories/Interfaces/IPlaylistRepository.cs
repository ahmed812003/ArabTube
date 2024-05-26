using ArabTube.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface IPlaylistRepository : IGenericRepository<Playlist>
    {
        Task<IEnumerable<Playlist>> SearchPlaylistAsync(string query);
        Task<IEnumerable<string>> SearchPlaylistTitlesAsync(string query);
        Task<bool> CheckDefultPlaylistsAsync(string userId);
        Task<IEnumerable<Playlist>> GetPlaylistsAsync(string userId , bool includePrivate);
        Task<string> FindPlaylistByNameAsync(string title, bool IsDefult , string userId);
        Task<bool> DeletePlaylistAsync(string playlistId);
        Task<bool> IsPlaylistNotSaved(string parentPlaylistId, string userId);
    }
}
