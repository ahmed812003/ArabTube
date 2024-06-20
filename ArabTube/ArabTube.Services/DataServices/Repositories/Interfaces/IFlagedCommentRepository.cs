using ArabTube.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface IFlagedCommentRepository : IGenericRepository<FlagedComment>
    {
        Task<bool> RemoveAsync(string commentId);
        Task<bool> RemoveRangeAsync(string videoId);
    }
}
