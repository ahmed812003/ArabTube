using ArabTube.Entities.DtoModels.VideoDTOs;
using ArabTube.Entities.VideoModels;

namespace ArabTube.Services.Interfaces
{
    public interface IVideoService
    {
        Task<IEnumerable<VideoQuality>> ProcessVideoAsync(ProcessingVideo model);
        Task<IEnumerable<string>> SearchVideoTitlesAsync(string query);
        Task<IEnumerable<VideoPreviewDto>> SearchVideoAsync(string query);
        Task<IEnumerable<VideoPreviewDto>> GetAllAsync();
        Task<ViewVideoDto> FindByIdAsync(string id);
        Task<bool> AddAsync(UploadingVideoDto video, string userName);
        Task<bool> LikeVideo(string id);
        Task<bool> DislikeVideo(string id);
        Task<bool> FlagVideo(string id);
        Task<bool> ViewVideo(string id);
        Task<bool> UpdateVideo(UpdatingVideoDto updateDto, string videoId);
        Task<bool> DeleteAsync(string id);
    }
}
