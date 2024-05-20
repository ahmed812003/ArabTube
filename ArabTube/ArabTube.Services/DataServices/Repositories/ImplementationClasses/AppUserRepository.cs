using ArabTube.Entities.Models;
using ArabTube.Services.DataServices.Data;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.ImplementationClasses
{
    public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
    {
        public AppUserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AppUser>> SearchUsersAsync(string query)
        {
            var users =await  _dbSet.Where(ap => ap.FirstName.Contains(query) || ap.LastName.Contains(query)
                                    || ap.UserName.Contains(query))
                                     .ToListAsync();
            return users;
        }

        public async Task<IEnumerable<string>> SearchChannelsAsync(string query)
        {
            var names = await _dbSet.Where(ap => ap.FirstName.Contains(query) || ap.LastName.Contains(query)
                                    || ap.UserName.Contains(query)).Select(ap => $"{ap.FirstName} {ap.LastName}" )
                                     .ToListAsync();
            return names;
        }
    }
}
