using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.CommentDTOs
{
    public class AddCommentDto
    {
        public string VideoId { get; set; }

        public string Content { get; set; }

        public string Mention { get; set; } = string.Empty;

        public string ParentCommentId { get; set; } = string.Empty;
    }
}
