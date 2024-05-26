using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.Models
{
    public class Playlist
    {
        public Playlist()
        {
            Videos = new List<Video>();
            PlaylistVideos = new List<PlaylistVideo>();
            Childrens = new List<Playlist>();
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsPrivate { get; set; } = true;

        public bool IsDefult { get; set; } = false;

        public string UserId { get; set; }

        public virtual AppUser User { get; set; }

        [Required]
        public string ParentPlaylistId { get; set; }

        public virtual Playlist ParentPlaylist { get; set; }
        public virtual ICollection<Playlist> Childrens { get; set; }


        public virtual ICollection<Video> Videos { get; set; }
        public virtual ICollection<PlaylistVideo> PlaylistVideos { get; set; }

    }
}
