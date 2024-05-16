using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.CommentDTOs
{
    public class AddCommentDto
    {
        [Required]
        public string VideoId { get; set; }

        [Required]
        public string Content { get; set; }
        [Required]
        public string Mention { get; set; } = string.Empty;
        [Required]
        public string ParentCommentId { get; set; } = string.Empty;
    }
}
