using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.PlaylistDTOs
{
    public class GetPlaylistDto
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public bool IsPrivate { get; set; }
    }
}
