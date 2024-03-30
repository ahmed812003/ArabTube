using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.Models
{
    public class AppUser : IdentityUser
    {
        [Required , MaxLength (250)]
        public string FirstName { get; set; }

        [Required , MaxLength(250)]
        public string LastName { get; set; }

        [Required]
        public bool Gender { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }
    }
}
