using ArabTube.Entities.DtoModels.UserDTOs;
using ArabTube.Entities.Models;
using AutoMapper;

namespace ArabTube.Entities.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterDto, AppUser>().ReverseMap();

            CreateMap<LoginDto, AppUser>().ReverseMap();

            CreateMap<AppUser, UserViewDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ChannelTitle, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }
    
}
