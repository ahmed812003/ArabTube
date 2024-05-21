using ArabTube.Services.DataServices.Data;
using ArabTube.Services.DataServices.Repositories.Interfaces;

namespace ArabTube.Services.DataServices.Repositories.ImplementationClasses
{
    public class UnitOfWork : IUnitOfWork
    {

        public IVideoRepository Video { get; }

        public IWatchedVideoRepository WatchedVideo { get; }

        public IAppUserConnectionRepository AppUserConnection { get; }

        public IPlaylistRepository Playlist { get; }

        public IPlaylistVideoRepository PlaylistVideo { get; }

        public ICommentRepository Comment { get; }

        public IAppUserRepository AppUser { get; }

        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {   
            _context = context;
            Video = new VideoRepository(_context);
            WatchedVideo = new WatchedVideoRepository(_context);
            AppUserConnection = new AppUserConnectionRepository(_context);
            Playlist = new PlaylistRepository(_context);
            PlaylistVideo = new PlaylistVideoRepository(_context);
            Comment = new CommentRepository(_context);
            AppUser = new AppUserRepository(_context);
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
