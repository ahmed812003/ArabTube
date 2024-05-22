using ArabTube.Entities.DtoModels.CommentDTOs;
using ArabTube.Entities.GenericModels;


namespace ArabTube.Entities.CommentModels
{
    public class GetVideoCommentsResult : ProcessResult
    {
        public IEnumerable<GetCommentDto> Comments { get; set; } = new List<GetCommentDto>();
    }
}
