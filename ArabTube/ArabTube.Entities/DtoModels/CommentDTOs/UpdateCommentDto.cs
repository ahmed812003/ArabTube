using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.CommentDTOs
{
    public class UpdateCommentDto
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public string CommentId { get; set; }
    }
}
