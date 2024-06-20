using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ArabTube.Entities.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            Videos = new List<Video>();
            History = new List<WatchedVideo>();
            WatchedVideos = new List<Video>();
            Followers = new List<AppUserConnection>();
            Following = new List<AppUserConnection>();
            Playlists = new List<Playlist>();
            Comments = new List<Comment>();
            VideosLikes = new List<VideoLike>();
            LikedVideos = new List<Video>();
            VideosDislikes = new List<VideoDislike>();
            DislikedVideos = new List<Video>();
            VideosFlags = new List<VideoFlag>();
            FlagedVideos = new List<Video>();
            CommentsLikes = new List<CommentLike>();
            LikedComments = new List<Comment>();
            CommentsDislikes = new List<CommentDislike>();
            DislikedComments = new List<Comment>();
            Notifications = new List<Notification>();
        }

        [Required, MaxLength(250)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(250)]
        public string LastName { get; set; } = string.Empty;

        public byte[]? ProfilePic { get; set; }

        public int NumberOfvideos { get; set; }

        public int NumberOfFollowers { get; set; }

        public virtual ICollection<Video> Videos { get; set; }

        public virtual ICollection<Video> WatchedVideos { get; set; }
        public virtual ICollection<WatchedVideo> History { get; set; }

        public virtual ICollection<AppUserConnection> Followers { get; set; } 
        public virtual ICollection<AppUserConnection> Following { get; set; } 

        public virtual ICollection<Playlist> Playlists { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<VideoLike> VideosLikes { get; set; }
        public virtual ICollection<Video> LikedVideos { get; set; }

        public virtual ICollection<VideoDislike> VideosDislikes { get; set; }
        public virtual ICollection<Video> DislikedVideos { get; set; }

        public virtual ICollection<VideoFlag> VideosFlags { get; set; }
        public virtual ICollection<Video> FlagedVideos { get; set; }

        public virtual ICollection<CommentLike> CommentsLikes { get; set; }
        public virtual ICollection<Comment> LikedComments { get; set; }

        public virtual ICollection<CommentDislike> CommentsDislikes { get; set; }
        public virtual ICollection<Comment> DislikedComments { get; set; }

        public virtual ICollection<CommentFlag> CommentsFlags { get; set; }
        public virtual ICollection<Comment> FlagedComments { get; set; }

        public ICollection<Notification> Notifications { get; set; }
    }
}
