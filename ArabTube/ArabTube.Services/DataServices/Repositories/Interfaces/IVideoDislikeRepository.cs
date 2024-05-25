using ArabTube.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface IVideoDislikeRepository : IGenericRepository<VideoDislike>
    {
        Task<bool> CheckUserDislikeVideoOrNotAsync(string userId, string videoId);
        Task<bool> RemoveUserVideoDislikeAsync(string userId, string videoId);
    }
}
