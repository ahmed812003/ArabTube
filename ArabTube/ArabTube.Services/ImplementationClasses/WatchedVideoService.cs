using ArabTube.Services.DataServices.Repositories.Interfaces;
using ArabTube.Services.Interfaces;

namespace ArabTube.Services.ImplementationClasses
{
    public class WatchedVideoService:IWatchedVideoService
    {
        private readonly IUnitOfWork _unitOfWork;
        public WatchedVideoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddWatchedVideoToHistoryAsync(string userId, string videoId)
        {
            var result = await _unitOfWork.WatchedVideo.AddWatchedVideoToHistoryAsync(userId, videoId);
            if (!result)
            {
                return false;
            }
            return true;
        }

        public Task<bool> DeleteWatchedVideoAsync(string videoId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
