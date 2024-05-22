using ArabTube.Entities.DtoModels.VideoDTOs;
using ArabTube.Entities.GenericModels;
using ArabTube.Entities.VideoModels;

namespace ArabTube.Services.ControllersServices.VideoServices.Interfaces
{
    public interface IVideoService
    {
        Task<IEnumerable<VideoQuality>> ProcessVideoAsync(ProcessingVideo model);
        Task<SearchVideoTitlesResult> SearchVideoTitlesAsync(string query);
        Task<GetVideoResult> SearchVideoAsync(string query);
        Task<GetVideoResult> PreviewVideoAsync();
        Task<WatchVideoResult> WatchVideoAsync(string id);
        Task<ProcessResult> UploadVideoAsync(UploadingVideoDto video, string userName);
        Task<ProcessResult> LikeVideoAsync(string id);
        Task<ProcessResult> DislikeVideoAsync(string id);
        Task<ProcessResult> FlagVideoAsync(string id);
        Task<ProcessResult> ViewVideoAsync(string id);
        Task<ProcessResult> UpdateVideoAsync(UpdatingVideoDto updateDto, string videoId);
        Task<ProcessResult> DeleteAsync(string id);
    }
}
