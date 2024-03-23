using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.UserDTOs
{
    public class LoginModel
    {
        [EmailAddress]
        [Required , MaxLength(255)]
        public string Email { get; set; }

        [Required , MaxLength(50)]
        public string Password { get; set; }
    }
}
