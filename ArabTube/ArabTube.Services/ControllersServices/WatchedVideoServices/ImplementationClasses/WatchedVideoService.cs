using ArabTube.Entities.DtoModels.WatchedVideoDto;
using ArabTube.Entities.GenericModels;
using ArabTube.Entities.HistoryModels;
using ArabTube.Services.ControllersServices.WatchedVideoServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using AutoMapper;

namespace ArabTube.Services.ControllersServices.WatchedVideoServices.ImplementationClasses
{
    public class WatchedVideoService : IWatchedVideoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WatchedVideoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetWatchedVideosResult> GetWatchedVideosAsync(string userId)
        {
            var watchedVideos = await _unitOfWork.WatchedVideo.GetWatchedVideosAsync(userId);

            if (watchedVideos == null || !watchedVideos.Any())
            {
                return new GetWatchedVideosResult { Message = "The Current Login User Dosn't Watch Any Video" };
            }
                
            var HistoryVideos = _mapper.Map<IEnumerable<HistoryVideoDto>>(watchedVideos);

            return new GetWatchedVideosResult
            {
                IsSuccesed = true,
                Videos = HistoryVideos
            };
        }

        public async Task<ProcessResult> RemoveFromHistoryAsync(string videoId , string userId)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(videoId);
            if (video == null)
            {
                return new ProcessResult { Message = $"No Video Exists With id = {videoId}" };
            }
            
            var result = await _unitOfWork.WatchedVideo.DeleteWatchedVideoAsync(videoId, userId);
            if (!result)
            {
                return new ProcessResult { Message = $"No Video Exists With id = {videoId}" };
            }

            await _unitOfWork.Complete();
            return new ProcessResult { IsSuccesed = true };
        }

        public async Task<ProcessResult> AddWatchedVideoToHistoryAsync(string userId, string videoId)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(videoId);
            if(video == null)
            {
                return new ProcessResult { Message = $"No Video Exists With id = {videoId}" };
            }
            var result = await _unitOfWork.WatchedVideo.AddWatchedVideoToHistoryAsync(userId, videoId);
            if (!result)
            {
                return new ProcessResult { Message = $"Error While Add video to history" };
            }
            await _unitOfWork.Complete();
            return new ProcessResult { IsSuccesed = true };
        }

    }
}
