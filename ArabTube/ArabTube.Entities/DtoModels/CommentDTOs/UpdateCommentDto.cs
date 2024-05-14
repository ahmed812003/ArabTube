using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.CommentDTOs
{
    public class UpdateCommentDto
    {
        public string Content { get; set; }

        public string CommentId { get; set; }
    }
}
