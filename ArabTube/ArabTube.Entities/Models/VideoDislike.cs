using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.Models
{
    public class VideoDislike
    {
        public string UserId { get; set; }

        public string VideoId { get; set; } 

        public virtual AppUser User { get; set; }

        public virtual Video Video { get; set; }
    }
}
