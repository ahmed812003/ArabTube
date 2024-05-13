using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.PlaylistDTOs
{
    public class UpdatePlaylistDto
    {
        [Required]
        public string PlaylistId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;
    }
}
