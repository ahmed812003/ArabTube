using ArabTube.Entities.DtoModels.PlaylistDTOs;
using ArabTube.Entities.Enums;
using ArabTube.Entities.GenericModels;
using ArabTube.Entities.Models;
using ArabTube.Entities.PlaylistModels;
using ArabTube.Services.ControllersServices.PlaylistServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ArabTube.Services.ControllersServices.PlaylistServices.ImplementationClasses
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public PlaylistService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<SearchPlaylistsTitlesResult> SearchPlaylistsTitlesAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new SearchPlaylistsTitlesResult { Message = "Query Cannot Be Empty" };

            var titles = await _unitOfWork.Playlist.SearchPlaylistTitlesAsync(query);

            if (!titles.Any())
                return new SearchPlaylistsTitlesResult { Message = "No Titles Found Matching The Search Query" };

            return new SearchPlaylistsTitlesResult
            {
                IsSuccesed = true,
                Titles = titles
            };
        }

        public async Task<GetPlaylistsResult> SearchPlaylistsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new GetPlaylistsResult { Message = "Query Cannot Be Empty" };

            var playlists = await _unitOfWork.Playlist.SearchPlaylistAsync(query);

            if (!playlists.Any())
                return new GetPlaylistsResult { Message = "No Videos Found Matching The Search Query" };
            
            IEnumerable<GetPlaylistDto> playlistsDto = _mapper.Map<IEnumerable<GetPlaylistDto>>(playlists);
           
            return new GetPlaylistsResult
            {
                IsSuccesed = true,
                Playlists = playlistsDto
            };
        }

        public async Task<GetPlaylistsResult> GetMyPlaylistsAsync(string userId)
        {
            var playlists = await _unitOfWork.Playlist.GetPlaylistsAsync(userId, true);

            if (!playlists.Any())
                return new GetPlaylistsResult { Message = "You Don't Have Any Playlist" };

             
            IEnumerable<GetPlaylistDto> playlistsDto = _mapper.Map<IEnumerable<GetPlaylistDto>>(playlists);
           
            return new GetPlaylistsResult
            {
                IsSuccesed = true,
                Playlists = playlistsDto
            };
        }

        public async Task<GetPlaylistsResult> GetPlaylistsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new GetPlaylistsResult { Message = $"No User With Id = {userId}" };
            }

            var playlists = await _unitOfWork.Playlist.GetPlaylistsAsync(userId, false);

            if (!playlists.Any())
                return new GetPlaylistsResult { Message = $"The User With Id = {userId} Dosn't Has Any Playlist" };

            IEnumerable<GetPlaylistDto> playlistsDto = _mapper.Map<IEnumerable<GetPlaylistDto>>(playlists);

            return new GetPlaylistsResult
            {
                IsSuccesed = true,
                Playlists = playlistsDto
            };
        }

        public async Task<ProcessResult> CreatePlaylistAsync(CreatePlaylistDto model, string userId)
        {
            Playlist playlist = _mapper.Map<Playlist>(model);

            var result = await _unitOfWork.Playlist.AddAsync(playlist);
            if (!result)
                return new ProcessResult { Message = "Error While Try To Create new Playlist" };
            await _unitOfWork.Complete();

            return new ProcessResult { IsSuccesed = true };
        }

        public async Task<ProcessResult> UpdatePlaylistAsync(UpdatePlaylistDto model , string userId)
        {
            var playlist = await _unitOfWork.Playlist.FindByIdAsync(model.PlaylistId);
            if (playlist == null)
            {
                return new ProcessResult { Message = $"No Playlist With Id = {model.PlaylistId}" };
            }

            if(playlist.UserId != userId)
            {
                return new ProcessResult { Message = $"Unauthorized to update this playlist" };
            }

            if (playlist.IsDefult)
            {
                return new ProcessResult { Message = "Can't Update Defult Playlists" };
            }

            _mapper.Map(model, playlist);
         
            await _unitOfWork.Complete();

            return new ProcessResult { IsSuccesed = true };
        }

        public async Task<ProcessResult> DeletePlaylistAsync(string playlistId , string userId)
        {
            var playlist = await _unitOfWork.Playlist.FindByIdAsync(playlistId);
            if (playlist == null)
            {
                return new ProcessResult { Message = $"No Playlist With Id = {playlist}" };
            }

            if(playlist.UserId != userId)
            {
                return new ProcessResult { Message = $"Unauthorized to delete this playlist" };
            }

            if (playlist.IsDefult)
            {
                return new ProcessResult { Message = "Can't Delete Defult Playlists" };
            }

            await _unitOfWork.PlaylistVideo.DeletePlaylistVideosAsync(playlistId);
            await _unitOfWork.Playlist.DeletePlaylistAsync(playlistId);
            await _unitOfWork.Complete();

            return new ProcessResult { IsSuccesed = true };

        }

        public async Task<ProcessResult> CreateDefultPlaylistsAsync(string userId)
        {
            var isHasDefultPlaylists = await _unitOfWork.Playlist.CheckDefultPlaylistsAsync(userId);
            if (!isHasDefultPlaylists)
            {
                var playlistNames = PlaylistDefaultNames.PlaylistNames;
                foreach (var playlistName in playlistNames)
                {
                    var entity = new Playlist
                    {
                        Title = playlistName,
                        UserId = userId,
                        IsDefult = true
                    };
                    var result = await _unitOfWork.Playlist.AddAsync(entity);
                    if (!result)
                    {
                        return new ProcessResult { Message = "Cannot Create Defult Playlist" };
                    }
                }
            }
            await _unitOfWork.Complete();
            return new ProcessResult { IsSuccesed = true };
        }

        public async Task<string> GetPlaylistIdAsync(string playlistTitle, bool isDefaultPlaylist , string userId)
        {
            return await _unitOfWork.Playlist.FindPlaylistByNameAsync
                                              (PlaylistDefaultNames.PlaylistNames[0], true , userId);
        }

    }
}
