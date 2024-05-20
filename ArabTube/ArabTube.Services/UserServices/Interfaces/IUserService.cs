using ArabTube.Entities.DtoModels.UserDTOs;

namespace ArabTube.Services.UserServices.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<string>> GetChannelsName(string query);
        Task<IEnumerable<UserViewDto>> GetUsersAsync(string query);
    }
}
