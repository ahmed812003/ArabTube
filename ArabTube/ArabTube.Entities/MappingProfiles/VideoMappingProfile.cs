using ArabTube.Entities.DtoModels.VideoDTOs;
using ArabTube.Entities.Enums;
using ArabTube.Entities.Models;
using AutoMapper;

namespace ArabTube.Entities.MappingProfiles
{
    public class VideoMappingProfile : Profile
    {
        public VideoMappingProfile()
        {
            CreateMap<Video, VideoPreviewDto>()
                .ForMember(dest => dest.ChannelTitle, opt => opt.MapFrom(src => $"{src.AppUser.FirstName} {src.AppUser.LastName}"));


            CreateMap<Video, ViewVideoDto>()
                .ForMember(dest => dest.ChannelTitle, opt => opt.MapFrom(src => $"{src.AppUser.FirstName} {src.AppUser.LastName}"))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.AppUser.Id))
                .ForMember(dest => dest.VideoUriList, opt => opt.MapFrom<VideoUriListResolver>());

            CreateMap<UploadingVideoDto, Video>()
                .AfterMap(async (src, dest) =>
                {
                    using var stream = new MemoryStream();
                    await src.Thumbnail.CopyToAsync(stream);
                    dest.Thumbnail = stream.ToArray();
                });
        }
    }
    public class VideoUriListResolver : IValueResolver<Video, ViewVideoDto, List<string>>
    {
        public List<string> Resolve(Video source, ViewVideoDto destination, List<string> destMember, ResolutionContext context)
        {
            var resolutions = Enum.GetValues(typeof(VideoResolutions));
            var videoUriList = new List<string>();

            foreach (var resolution in resolutions)
            {
                videoUriList.Add($"{source.VideoUri}{(int)resolution}");
            }

            return videoUriList;
        }

    }
}
