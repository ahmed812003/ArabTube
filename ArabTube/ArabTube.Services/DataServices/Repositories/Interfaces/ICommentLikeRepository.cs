using ArabTube.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface ICommentLikeRepository : IGenericRepository<CommentLike>
    {
        Task<bool> CheckUserLikeCommentOrNotAsync(string commentId, string userId);
        Task<bool> RemoveUserCommentLikeAsync(string commentId, string userId);
    }
}
