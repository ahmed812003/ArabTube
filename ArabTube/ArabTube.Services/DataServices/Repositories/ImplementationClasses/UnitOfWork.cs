using ArabTube.Services.DataServices.Data;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.DataServices.Repositories.ImplementationClasses
{
    public class UnitOfWork : IUnitOfWork
    {

        public IVideoRepository Video { get; }

        public IWatchedVideoRepository WatchedVideo { get; }

        public IAppUserConnectionRepository AppUserConnection { get; }

        public IPlaylistRepository Playlist { get; }

        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Video = new VideoRepository(_context);
            WatchedVideo = new WatchedVideoRepository(_context);
            AppUserConnection = new AppUserConnectionRepository(_context);
            Playlist = new PlaylistRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task Complete()
        {
            await _context.SaveChangesAsync();
        }
    }
}
