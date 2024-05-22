using ArabTube.Entities.GenericModels;
using ArabTube.Entities.HistoryModels;

namespace ArabTube.Services.ControllersServices.WatchedVideoServices.Interfaces
{
    public interface IWatchedVideoService
    {
        Task<GetWatchedVideosResult> GetWatchedVideosAsync(string userId);
        Task<ProcessResult> RemoveFromHistoryAsync(string videoId, string userId);
        Task<ProcessResult> AddWatchedVideoToHistoryAsync(string userId, string videoId);
    }
}
