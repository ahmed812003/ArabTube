namespace ArabTube.Services.Interfaces
{
    public interface IPlaylistVideoService
    {
        Task<bool> AddVideoToPlayListAsync(string videoId,string playlistId);
        Task<bool> FindVideoInPlaylistAsync(string videoId,string playlistId);
        Task<bool> RemoveVideoFromPlaylistAsync(string videoId, string playlistId);
    }
}
