using Microsoft.AspNetCore.Identity.UI.Services;

namespace ArabTube.Services.Interfaces
{
    public interface IUnitOfService
    {
        IUserService AppUser { get; }
        IVideoService Video { get; }
        IWatchedVideoService WatchedVideo { get; }
        IPlaylistService Playlist { get; }
        IPlaylistVideoService PlaylistVideo { get; }
        ICommentService Comment { get; }
        IAuthService Auth { get; }
        ICloudService Cloud { get; }
        IEmailSender EmailSender { get; }
    }
}
