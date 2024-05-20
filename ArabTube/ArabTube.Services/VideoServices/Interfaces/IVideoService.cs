using ArabTube.Entities.DtoModels.VideoDTOs;
using ArabTube.Entities.VideoModels;

namespace ArabTube.Services.VideoServices.Interfaces
{
    public interface IVideoService
    {
        Task<IEnumerable<VideoQuality>> ProcessVideoAsync(ProcessingVideo model);
        Task<IEnumerable<string>> SearchVideoTitlesAsync(string query);
        Task<IEnumerable<VideoPreviewDto>> SearchVideoAsync(string query);
        Task<IEnumerable<VideoPreviewDto>> GetAllAsync();
        Task<ViewVideoDto> FindByIdAsync(string id);
        Task<bool> AddAsync(UploadingVideoDto video,string userName);
    }
}
