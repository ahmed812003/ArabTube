using ArabTube.Entities.DtoModels.PlaylistDTOs;
using ArabTube.Entities.GenericModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.PlaylistModels
{
    public class GetPlaylistVideosResult : ProcessResult
    {
        public IEnumerable<PlaylistVideoDto> Videos { get; set; } = new List<PlaylistVideoDto>();
    }
}
