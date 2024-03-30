using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.Models
{
    /// <summary>
    /// self relationship => on delete restrict.
    /// others => on delete cascade.
    /// don't forget that when you implement delete comment endpoint
    /// </summary>
    public class Comment
    {
        public Comment()
        {
            AppUser = new AppUser();  
            Video = new Video();
            Childrens = new HashSet<Comment>();
        }

        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public bool IsUpdated { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string VideoId { get; set; } = string.Empty;

        [Required]
        public string? ParentCommentId { get; set; } = string.Empty;

        public virtual AppUser AppUser { get; set; }

        public virtual Video Video { get; set; }

        public virtual Comment ParentComment { get; set; }

        public virtual ICollection<Comment> Childrens { get; set;}

    }
}
