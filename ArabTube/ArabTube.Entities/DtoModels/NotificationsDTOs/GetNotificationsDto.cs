using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.NotificationsDTOs
{
    public class GetNotificationsDto
    {
        public string Message { get; set; }
        public DateTime SendTime { get; set; }
        public string ChannelTitle { get; set; }
        public string UserId { get; set; }
    }
}
