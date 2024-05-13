using ArabTube.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface IWatchedVideoRepository:IGenericRepository<WatchedVideo>
    {
        
        Task<IEnumerable<WatchedVideo>?> GetWatchedVideosAsync(string userId);
        Task<bool> AddWatchedVideoToHistoryAsync(string userId, string videoId);

        Task<bool> DeleteWatchedVideoAsync(string videoId , string userId);
    }
}
