using ArabTube.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.DtoModels.CommentDTOs
{
    public class GetCommentDto
    {
        public string commentId { get; set; }
        public string Username { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsUpdated { get; set; }

        public string Mention { get; set; }

        public int Likes { get; set; }

        public int DisLike { get; set; }

        public List<GetCommentDto> childrens { get; set; } = new List<GetCommentDto>();


    }
}
