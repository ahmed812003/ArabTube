using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.Models
{
    public class FlagedComment
    {
        public string Id {  get; set; } = Guid.NewGuid().ToString();

        public string CommentId { get; set; }

        public string VideoId { get; set; }
    }
}
