using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.UserDTOs
{
    public class ResendEmailConfirmationDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
