using ArabTube.Entities.DtoModels.UserDTOs;
using ArabTube.Entities.GenericModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.UserModels
{
    public class GetChannelsResult : ProcessResult
    {
        public List<GetChannelDto> Channels { get; set; } = new List<GetChannelDto>();
    }
}
