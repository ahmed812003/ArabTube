using ArabTube.Entities.GenericModels;
using ArabTube.Entities.PlaylistModels;

namespace ArabTube.Services.ControllersServices.PlaylistVideoServices.Interfaces
{
    public interface IPlaylistVideoService
    {
        Task<ProcessResult> AddVideoToPlayListAsync(string videoId, string playlistId , string userId);
        Task<GetPlaylistVideosResult> GetPlaylistVideosAsync(string playlistId);
        Task<bool> FindVideoInPlaylistAsync(string videoId, string playlistId);
        Task<ProcessResult> RemoveVideoFromPlaylistAsync(string videoId, string playlistId , string userId);
    }
}
