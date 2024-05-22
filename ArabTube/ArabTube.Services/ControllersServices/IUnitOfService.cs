using ArabTube.Services.ControllersServices.AuthServices.Interfaces;
using ArabTube.Services.ControllersServices.CloudServices.Interfaces;
using ArabTube.Services.ControllersServices.CommentServices.Interfaces;
using ArabTube.Services.ControllersServices.PlaylistServices.Interfaces;
using ArabTube.Services.ControllersServices.PlaylistVideoServices.Interfaces;
using ArabTube.Services.ControllersServices.SubscriptionServices.Interfaces;
using ArabTube.Services.ControllersServices.UserServices.Interfaces;
using ArabTube.Services.ControllersServices.VideoServices.Interfaces;
using ArabTube.Services.ControllersServices.WatchedVideoServices.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ArabTube.Services.ControllersServices
{
    public interface IUnitOfService : IDisposable
    {
        ICommentService Comment { get; }
        IAuthService AuthService { get; }
        ICloudService CloudService { get; }
        IEmailSender EmailSender { get; }
        IPlaylistService PlaylistService { get; }
        IPlaylistVideoService PlaylistVideoService { get; }
        ISubscriptionService SubscriptionService { get; }
        IUserService UserService { get; }
        IVideoService VideoService { get; }
        IWatchedVideoService WatchedVideoService { get; }

    }
}
