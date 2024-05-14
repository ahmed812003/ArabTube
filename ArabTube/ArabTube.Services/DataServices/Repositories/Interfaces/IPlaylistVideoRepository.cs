using ArabTube.Entities.Models;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface IPlaylistVideoRepository:IGenericRepository<PlaylistVideo>
    {
        Task<IEnumerable<PlaylistVideo>?> GetPlaylistVideosAsync(string playlistId);

        Task AddVideoToPlayListAsync(string videoId, string playlistId);

        Task RemoveVideoFromPlayListAsync(string videoId, string playlistId);
    }
}
