using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.VideoDTOs
{
    public class UploadingVideoDto
    {
        [Required, MaxLength(256)]
        public string Title { get; set; } = string.Empty;
        [Required, MaxLength(2000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public IFormFile Thumbnail { get; set; }
        [Required]
        public IFormFile Video { get; set; }
    }
}
