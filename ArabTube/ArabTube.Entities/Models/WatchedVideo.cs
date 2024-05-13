using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.Models
{
    public class WatchedVideo
    {
        public WatchedVideo()
        {
            WatchedTime = DateTime.Now;
        }

        public string UserId { get; set; } = string.Empty;

        public string VideoId { get; set; } = string.Empty;

        public DateTime WatchedTime { get; set; }

        public virtual AppUser User { get; set; }

        public virtual Video Video { get; set; }

    }
}
