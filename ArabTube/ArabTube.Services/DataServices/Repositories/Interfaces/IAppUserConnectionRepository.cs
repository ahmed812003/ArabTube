using ArabTube.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface IAppUserConnectionRepository : IGenericRepository<AppUserConnection>
    {
        Task<IEnumerable<AppUserConnection>> GetFollowingAsync(string id);
        Task<bool> SubscribeAsync(string ownerId, string userId);
        Task<bool> UnSubscribeAsync(string ownerId, string userId);
    }
}
