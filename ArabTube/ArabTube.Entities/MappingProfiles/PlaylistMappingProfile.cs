using ArabTube.Entities.DtoModels.PlaylistDTOs;
using ArabTube.Entities.Models;
using AutoMapper;

namespace ArabTube.Entities.MappingProfiles
{
    public class PlaylistMappingProfile :Profile
    {
        public PlaylistMappingProfile()
        {
            CreateMap<Playlist, GetPlaylistDto>();
            CreateMap<CreatePlaylistDto, Playlist>();
            CreateMap<UpdatePlaylistDto, Playlist>();
        }
    }
}
