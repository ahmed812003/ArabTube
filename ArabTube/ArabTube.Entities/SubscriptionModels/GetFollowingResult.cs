using ArabTube.Entities.DtoModels.UserConnectionsDto;
using ArabTube.Entities.GenericModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.SubscriptionModels
{
    public class GetFollowingResult : ProcessResult
    {
        public IEnumerable<FollowingDto> Followings { get; set; } = new List<FollowingDto>();
    }
}
