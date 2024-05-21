using ArabTube.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ArabTube.Services.ImplementationClasses
{
    public class UnitOfService : IUnitOfService
    {
        public UnitOfService(IUserService appUser, IVideoService video, IWatchedVideoService watchedVideo, 
            IPlaylistService playlist, IPlaylistVideoService playlistVideo, ICommentService comment,
            IAuthService auth, ICloudService cloud, IEmailSender emailSender)
        {
            AppUser = appUser;
            Video = video;
            WatchedVideo = watchedVideo;
            Playlist = playlist;
            PlaylistVideo = playlistVideo;
            Comment = comment;
            Auth = auth;
            Cloud = cloud;
            EmailSender = emailSender;
        }

        public IUserService AppUser { get; }
        public IVideoService Video { get; }
        public IWatchedVideoService WatchedVideo { get; }
        public IPlaylistService Playlist { get; }
        public IPlaylistVideoService PlaylistVideo { get; }
        public ICommentService Comment { get; }
        public IAuthService Auth { get; }
        public ICloudService Cloud { get; }
        public IEmailSender EmailSender { get; }

    }
}