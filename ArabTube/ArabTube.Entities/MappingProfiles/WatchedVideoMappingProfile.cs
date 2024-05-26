using ArabTube.Entities.DtoModels.WatchedVideoDto;
using ArabTube.Entities.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.MappingProfiles
{
    public class WatchedVideoMappingProfile : Profile
    {
        public WatchedVideoMappingProfile() 
        {
            CreateMap<WatchedVideo, HistoryVideoDto>()
                .ForMember(dest => dest.ChannelTitle, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Video.Title))
                .ForMember(dest => dest.Views, opt => opt.MapFrom(src => src.Video.Views))
                .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.Video.Thumbnail));
        }
    }
}
