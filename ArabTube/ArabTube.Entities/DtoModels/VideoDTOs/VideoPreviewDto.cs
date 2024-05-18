using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.VideoDTOs
{
    public class VideoPreviewDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public int Likes { get; set; }

        public int DisLikes { get; set; }

        public int Views { get; set; }

        public string ChannelTitle { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public byte[] Thumbnail { get; set; }
    }
}
