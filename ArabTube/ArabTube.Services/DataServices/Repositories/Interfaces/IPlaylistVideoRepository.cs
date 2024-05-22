using ArabTube.Entities.Models;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface IPlaylistVideoRepository:IGenericRepository<PlaylistVideo>
    {
        Task<IEnumerable<PlaylistVideo>> GetPlaylistVideosAsync(string playlistId);
        Task<bool> FindVideoInPlaylistAsync(string videoId, string playlistId);
        Task<bool> AddVideoToPlayListAsync(string videoId, string playlistId);
        Task DeletePlaylistVideosAsync(string playlistId);
        Task<bool> RemoveVideoFromPlayListAsync(string videoId, string playlistId);
    }
}
