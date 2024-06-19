using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.UserDTOs
{
    public class UserViewDto
    {
        public string UserId { get; set; }
        public string ChannelTitle { get; set; }
        public string UserName { get; set; }
        public byte[] ProfilePic { get; set; }
    }
}
