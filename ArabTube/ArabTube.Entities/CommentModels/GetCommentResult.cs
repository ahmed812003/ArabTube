using ArabTube.Entities.DtoModels.CommentDTOs;
using ArabTube.Entities.GenericModels;

namespace ArabTube.Entities.CommentModels
{
    public class GetCommentResult : ProcessResult
    {
        public GetCommentDto Comment { get; set; }
    }
}
