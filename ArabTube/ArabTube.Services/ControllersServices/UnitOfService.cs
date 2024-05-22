using ArabTube.Services.ControllersServices.AuthServices.ImplementationClasses;
using ArabTube.Services.ControllersServices.AuthServices.Interfaces;
using ArabTube.Services.ControllersServices.CloudServices.Interfaces;
using ArabTube.Services.ControllersServices.CommentServices.ImplementationClasses;
using ArabTube.Services.ControllersServices.CommentServices.Interfaces;
using ArabTube.Services.ControllersServices.PlaylistServices.Interfaces;
using ArabTube.Services.ControllersServices.PlaylistVideoServices.Interfaces;
using ArabTube.Services.ControllersServices.SubscriptionServices.Interfaces;
using ArabTube.Services.ControllersServices.UserServices.Interfaces;
using ArabTube.Services.ControllersServices.VideoServices.Interfaces;
using ArabTube.Services.ControllersServices.WatchedVideoServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ArabTube.Services.ControllersServices
{
    public class UnitOfService : IUnitOfService
    {
        public ICommentService Comment { get; }

        public IAuthService AuthService { get; }

        public ICloudService CloudService { get; }

        public IEmailSender EmailSender { get; }

        public IPlaylistService PlaylistService { get; }

        public IPlaylistVideoService PlaylistVideoService { get; }

        public ISubscriptionService SubscriptionService { get; }

        public IUserService UserService { get; }

        public IVideoService VideoService { get; }

        public IWatchedVideoService WatchedVideoService { get; }

        private readonly IUnitOfWork _unitOfWork;

        public UnitOfService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            /*Comment = new CommentService(unitOfWork);
            AuthService = new AuthService(unitOfWork);
            CloudService { get; }
            EmailSender { get; }
            PlaylistService { get; }
            PlaylistVideoService { get; }
            SubscriptionService { get; }
            UserService { get; }
            VideoService { get; }
            WatchedVideoService { get; }*/
        }



        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
