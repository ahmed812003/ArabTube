using ArabTube.Entities.DtoModels.VideoDTOs;
using ArabTube.Entities.GenericModels;
using ArabTube.Entities.Models;
using ArabTube.Entities.VideoModels;

namespace ArabTube.Services.ControllersServices.VideoServices.Interfaces
{
    public interface IVideoService
    { 
        Task<SearchVideoTitlesResult> SearchVideoTitlesAsync(string query);
        Task<GetVideoResult> SearchVideoAsync(string query);
        Task<GetVideoResult> PreviewVideoAsync();
        Task<WatchVideoResult> WatchVideoAsync(string id);
        Task<GetVideoResult> UserVideosAsync(string userId);
        Task<ProcessResult> IsUserLikeVideoAsync(string videoId, string userId);
        Task<ProcessResult> IsUserDisLikeVideoAsync(string videoId, string userId);
        Task<ProcessResult> IsUserFlagVideoAsync(string videoId, string userId);
        Task<ProcessResult> UploadVideoAsync(UploadingVideoDto video, string userName , AppUser user);
        Task<ProcessResult> LikeVideoAsync(string id , string userId);
        Task<ProcessResult> DislikeVideoAsync(string id , string userId);
        Task<ProcessResult> FlagVideoAsync(string id , string userId);
        Task<ProcessResult> ViewVideoAsync(string id);
        Task<ProcessResult> UpdateVideoAsync(UpdatingVideoDto updateDto, string videoId , string userId);
        Task<ProcessResult> DeleteAsync(string id , AppUser user);
    }
}
