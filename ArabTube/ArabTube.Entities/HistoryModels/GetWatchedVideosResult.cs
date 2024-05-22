using ArabTube.Entities.DtoModels.WatchedVideoDto;
using ArabTube.Entities.GenericModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.HistoryModels
{
    public class GetWatchedVideosResult : ProcessResult
    {
        public IEnumerable<HistoryVideoDto> Videos { get; set; } = new List<HistoryVideoDto>();
    }
}
