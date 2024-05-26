using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.WatchedVideoDto
{
    public class HistoryVideoDto
    {
        public string VideoId { get; set; }
        public string Title { get; set; }
        public int Views { get; set; }
        public DateTime WatchedTime { get; set; }
        public string UserId { get; set; }
        public string ChannelTitle { get; set; }
        public string Username { get; set; }
        public byte[] Thumbnail { get; set; }
    }
}
