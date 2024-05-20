using ArabTube.Entities.Models;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface IAppUserRepository : IGenericRepository<AppUser>
    {
        Task<IEnumerable<AppUser>> SearchUsersAsync(string query);
        Task<IEnumerable<string>> SearchChannelsAsync(string query);
    }
}
