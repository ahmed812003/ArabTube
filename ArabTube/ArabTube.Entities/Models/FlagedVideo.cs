using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.Models
{
    public class FlagedVideo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string VideoId { get; set; }

    }
}
