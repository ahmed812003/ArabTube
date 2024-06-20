using ArabTube.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface ICommentFlagRepository : IGenericRepository<CommentFlag>
    {
        Task<bool> CheckUserFlagCommentOrNotAsync(string commentId, string userId);
        Task<bool> RemoveUserCommentFlagAsync(string commentId, string userId);
    }
}
