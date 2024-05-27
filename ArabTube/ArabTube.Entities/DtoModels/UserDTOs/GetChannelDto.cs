using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.UserDTOs
{
    public class GetChannelDto
    {
        public string ChannelTitle { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
        public byte[] ProfilePic { get; set; }

    }
}
