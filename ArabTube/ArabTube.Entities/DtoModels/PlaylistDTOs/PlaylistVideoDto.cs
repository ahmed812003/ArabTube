using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.PlaylistDTOs
{
    public class PlaylistVideoDto
    {
        public string VideoId { get; set; }
        public string Title { get; set; }
        public int Views { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Username { get; set; }
        public byte[] Thumbnail { get; set; }
    }
}
