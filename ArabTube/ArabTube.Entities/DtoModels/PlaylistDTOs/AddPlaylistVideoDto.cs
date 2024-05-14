using System.ComponentModel.DataAnnotations;

namespace ArabTube.Entities.DtoModels.PlaylistDTOs
{
    public class AddPlaylistVideoDto
    {
        [Required]
        public string VideoID { get; set; }
        [Required]
        public string PlaylistID { get; set; }
    }
}
