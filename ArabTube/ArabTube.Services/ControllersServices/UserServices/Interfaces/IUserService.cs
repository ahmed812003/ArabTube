using ArabTube.Entities.DtoModels.UserDTOs;
using ArabTube.Entities.UserModels;

namespace ArabTube.Services.ControllersServices.UserServices.Interfaces
{
    public interface IUserService
    {
        Task<GetChannelsNameResult> GetChannelsNameAsync(string query);
        Task<GetUsersResult> GetUsersAsync(string query);
    }
}
