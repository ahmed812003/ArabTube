using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.PlaylistDTOs
{
    public class RemovePlaylistVideoDto
    {
        [Required]
        public string VideoId { get; set; }
        [Required]
        public string PlaylistId { get; set; }
    }
}
