using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.UserDTOs
{
    public class ResetPasswordDto
    {
        [Required, MaxLength(50)]
        public string newPassword { get; set; }

        public string UserCode { get; set; }

        public string ValidCode { get; set; }

        public string UserId { get; set; }
    }
}
