using ArabTube.Entities.DtoModels.VideoDTOs;
using ArabTube.Entities.GenericModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.VideoModels
{
    public class GetVideoResult : ProcessResult
    {
        public IEnumerable<VideoPreviewDto> Videos { get; set; } = new List<VideoPreviewDto>();
    }
}
