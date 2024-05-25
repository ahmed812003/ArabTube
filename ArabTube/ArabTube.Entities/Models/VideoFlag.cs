using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.Models
{
    public class VideoFlag
    {
        public string UserId { get; set; }
        public string VideoId { get; set; } 

        public virtual AppUser User { get; set; }
        public virtual Video Video { get; set; }

    }
}
