using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.Models
{
    public class Video
    {
        public Video()
        {
            Viewers = new List<AppUser>();
            History = new List<WatchedVideo>();
            Playlists = new List<Playlist>();
            PlaylistVideos = new List<PlaylistVideo>();
            Comments = new List<Comment>();
        }

        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required, MaxLength(256)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string VideoUri { get; set; } = string.Empty;

        public int Likes { get; set; }

        public int DisLikes { get; set; }

        public int Views { get; set; }

        public int Flags { get; set; }

        [Required]
        public byte[] Thumbnail { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime UpdatedOn { get; set; } = DateTime.Now;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public virtual AppUser AppUser { get; set; }


        public virtual ICollection<AppUser> Viewers { get; set; }
        public virtual ICollection<WatchedVideo> History { get; set; }


        public virtual ICollection<PlaylistVideo> PlaylistVideos { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

    }
}