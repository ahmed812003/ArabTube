using ArabTube.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface IVideoLikeRepository : IGenericRepository<VideoLike>
    {
        Task<bool> CheckUserLikeVideoOrNotAsync(string userId, string videoId);
        Task<bool> RemoveUserVideoLikeAsync(string userId, string videoId);
    }
}
