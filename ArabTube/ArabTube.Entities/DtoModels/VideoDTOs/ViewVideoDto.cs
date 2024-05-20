using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.VideoDTOs
{
    public class ViewVideoDto
    {
        public ViewVideoDto()
        {
            VideoUriList = new List<string>();
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> VideoUriList { get; set; }
        public int Likes { get; set; }
        public int DisLikes { get; set; }
        public int Views { get; set; }
        public int Flags { get; set; }
        public byte[] Thumbnail { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string ChannelTitle { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }


    }
}
