using ArabTube.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface IAppUserRepository : IGenericRepository<AppUser>
    {
        Task<IEnumerable<AppUser>> SearchUsersAsync(string query);
        Task<IEnumerable<string>> SearchUsersNamesAsync(string query);
    }
}
