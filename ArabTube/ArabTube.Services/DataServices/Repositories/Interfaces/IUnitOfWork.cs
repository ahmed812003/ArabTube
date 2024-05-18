using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAppUserRepository AppUser {get;}
        IVideoRepository Video { get; }
        IWatchedVideoRepository WatchedVideo { get; }
        IAppUserConnectionRepository AppUserConnection { get; }
        IPlaylistRepository Playlist { get; }
        IPlaylistVideoRepository PlaylistVideo { get; }
        ICommentRepository Comment { get; }
        Task Complete();
    }
}
