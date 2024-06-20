using ArabTube.Entities.DtoModels.CommentDTOs;
using ArabTube.Entities.GenericModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.CommentModels
{
    public class GetFlagedCommentsResult : ProcessResult
    {
        public List<GetCommentDto> getCommentDtos { get; set; } =  new List<GetCommentDto>();
    }
}
