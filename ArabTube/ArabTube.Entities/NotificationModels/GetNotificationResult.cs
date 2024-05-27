using ArabTube.Entities.DtoModels.NotificationsDTOs;
using ArabTube.Entities.GenericModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.NotificationModels
{
    public class GetNotificationResult : ProcessResult
    {
        public IEnumerable<GetNotificationsDto> NotificationsDtos { get; set; } = new List<GetNotificationsDto>();
    }
}
