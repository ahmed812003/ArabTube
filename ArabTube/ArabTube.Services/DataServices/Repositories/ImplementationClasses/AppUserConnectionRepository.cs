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
    public class AppUserConnectionRepository : GenericRepository<AppUserConnection>, IAppUserConnectionRepository
    {
        public AppUserConnectionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AppUserConnection>> GetFollowingAsync(string id)
        {
            var appUserConnections = await _dbSet.Where(auc => auc.FollowerId == id)
                .Include(auc => auc.Following).ToListAsync();
            return appUserConnections;
        }

        public async Task SubscribeAsync(string ownerId, string userId)
        {
            await _dbSet.AddAsync(new AppUserConnection
                  {
                    FollowerId = userId,
                    FollowingId = ownerId 
                  });
            return;
        }

        public async Task UnSubscribeAsync(string ownerId, string userId)
        {
            var entity = await _dbSet.FindAsync(userId , ownerId);
            if(entity != null)
            {
                _dbSet.Remove(entity);
            }
            return;
        }


    }
}
