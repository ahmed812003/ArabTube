using ArabTube.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface IcommentDislikeRepository : IGenericRepository<CommentDislike>
    {
        Task<bool> CheckUserDislikeCommentOrNotAsync(string commentId, string userId);
        Task<bool> RemoveUserCommentDislikeAsync(string commentId, string userId);
    }
}
