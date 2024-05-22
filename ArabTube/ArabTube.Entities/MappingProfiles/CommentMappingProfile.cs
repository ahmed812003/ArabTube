using ArabTube.Entities.DtoModels.CommentDTOs;
using ArabTube.Entities.Models;
using AutoMapper;

namespace ArabTube.Entities.MappingProfiles
{
    public class CommentMappingProfile:Profile
    {
        public CommentMappingProfile()
        {
            CreateMap<Comment, GetCommentDto>()
                .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.Childrens, opt => opt.MapFrom(src => src.Childrens.Where(cc => cc.Id != src.Id).OrderBy(cc => cc.CreatedOn)));

            CreateMap<AddCommentDto, Comment>()
              .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.Now))
              .ForMember(dest => dest.ParentCommentId, opt => opt.Ignore());

        }
    }
}
