using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.VideoDTOs
{
    public class UpdatingVideoDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile UpdateThumbnail { get; set; }
    }
}
