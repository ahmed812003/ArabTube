using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.Models
{
    public class CommentDislike
    {
        public string UserId { get; set; }

        public string CommentId { get; set; }

        public virtual AppUser User { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
