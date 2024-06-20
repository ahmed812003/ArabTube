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
        IVideoLikeRepository VideoLike { get; }
        IVideoDislikeRepository VideoDislike { get; }
        IVideoFlagRepository VideoFlag { get; }
        ICommentLikeRepository CommentLike { get; }
        IcommentDislikeRepository CommentDislike { get; }
        ICommentFlagRepository CommentFlag { get; }
        INotificationRepository Notification { get; }
        IFlagedVideoRepository FlagedVideo { get; }
        IFlagedCommentRepository FlagedComment { get; }
        Task Complete();
    }
}
