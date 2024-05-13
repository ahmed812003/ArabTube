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
        Task<IEnumerable<Playlist>> GetPlaylistsAsync(string userId , bool includePrivate);

        Task<bool> DeletePlaylistAsync(string playlistId , string userId);
    }
}
