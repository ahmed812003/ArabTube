using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.Models
{
    public class PlaylistVideo
    {
        public string VideoId { get; set; }

        public string PlaylistId { get; set; }
        
        public virtual Video Video { get; set; }

        public virtual Playlist Playlist { get; set; }
    }
}
