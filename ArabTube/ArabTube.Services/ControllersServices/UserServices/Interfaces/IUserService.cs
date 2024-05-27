using ArabTube.Entities.DtoModels.UserDTOs;
using ArabTube.Entities.GenericModels;
using ArabTube.Entities.Models;
using ArabTube.Entities.UserModels;

namespace ArabTube.Services.ControllersServices.UserServices.Interfaces
{
    public interface IUserService
    {
        Task<GetChannelsNameResult> GetChannelsNameAsync(string query);
        Task<GetUsersResult> GetUsersAsync(string query);
        Task<ProcessResult> SetProfilePicAsync(SetProfilePicDto model, AppUser user);
        Task<GetChannelResult> GetChannelsAsync(string userId);
    }
}
