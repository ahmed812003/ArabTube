using System.ComponentModel.DataAnnotations;

namespace ArabTube.Entities.DtoModels.PlaylistDTOs
{
    public class AddPlaylistVideoDto
    {
        [Required]
        public string VideoId { get; set; }
        [Required]
        public string PlaylistId { get; set; }
    }
}
