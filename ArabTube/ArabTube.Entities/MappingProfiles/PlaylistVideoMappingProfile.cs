using ArabTube.Entities.DtoModels.PlaylistDTOs;
using ArabTube.Entities.Models;
using AutoMapper;

namespace ArabTube.Entities.MappingProfiles
{
    public class PlaylistVideoMappingProfile:Profile
    {
        public PlaylistVideoMappingProfile()
        {
            CreateMap<PlaylistVideo, PlaylistVideoDto>()
              .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Video.Title))
              .ForMember(dest => dest.Views, opt => opt.MapFrom(src => src.Video.Views))
              .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.Video.CreatedOn))
              .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Video.AppUser.UserName))
              .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.Video.Thumbnail));
        }
    }
}
