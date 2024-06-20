using ArabTube.Entities.DtoModels.VideoDTOs;
using ArabTube.Entities.GenericModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.VideoModels
{
    public class GetFlagedVideosResult : ProcessResult
    {
        public IEnumerable<ViewVideoDto> viewVideoDtos { get; set; } = new List<ViewVideoDto>();
    }
}
