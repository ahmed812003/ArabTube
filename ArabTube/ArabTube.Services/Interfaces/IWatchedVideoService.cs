namespace ArabTube.Services.Interfaces
{
    public interface IWatchedVideoService
    {
        Task<bool> AddWatchedVideoToHistoryAsync(string userId, string videoId);
        Task<bool> DeleteWatchedVideoAsync(string videoId, string userId);
    }
}
