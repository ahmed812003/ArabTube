using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.Models
{
    public class Video
    {
        public Video()
        {
            Comments = new List<Comment>();    
        }

        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Url { get; set; } = string.Empty;

        public int Likes { get; set; }
        
        public int DisLikes { get; set; }

        public int Views { get; set; }

        public int Flags { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime UpdatedOn { get; set;} = DateTime.Now;

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
