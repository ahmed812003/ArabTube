using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.UserDTOs
{
    public class RoleDto
    {
        [Required]
        public string RoleName { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
