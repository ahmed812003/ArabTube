using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.UserDTOs
{
    public class EmailConfirmationDto
    {
        [Required]
        public string UserCode { get; set; }
        [Required]
        public string ValidCode { get; set; }
        [Required]
        public string UserId { get; set; }

    }
}
