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
        Task SubscribeAsync(string ownerId, string userId);
        Task UnSubscribeAsync(string ownerId, string userId);
    }
}
