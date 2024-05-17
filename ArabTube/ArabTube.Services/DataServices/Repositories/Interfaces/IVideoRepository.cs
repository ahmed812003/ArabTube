using ArabTube.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface IVideoRepository : IGenericRepository<Video>
    {
        Task<IEnumerable<string>> SearchVideoTitlesAsync(string query);

        Task<IEnumerable<Video>> SearchVideoAsync(string query);
    }
}
