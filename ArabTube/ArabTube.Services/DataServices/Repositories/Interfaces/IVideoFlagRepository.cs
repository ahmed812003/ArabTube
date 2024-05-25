using ArabTube.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface IVideoFlagRepository : IGenericRepository<VideoFlag>
    {
        Task<bool> CheckUserFlagVideoOrNotAsync(string userId, string videoId);
        Task<bool> RemoveUserVideoFlagAsync(string userId, string videoId);
    }
}
