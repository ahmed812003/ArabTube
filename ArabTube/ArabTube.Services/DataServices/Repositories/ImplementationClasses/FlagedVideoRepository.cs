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
    public class FlagedVideoRepository : GenericRepository<FlagedVideo>, IFlagedVideoRepository
    {
        public FlagedVideoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> RemoveAsync (string videoId)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(fv => fv.VideoId == videoId);

            if(entity == null)
            {
                return false;
            }

            _dbSet.Remove(entity);
            return true;
        }
    }
}
