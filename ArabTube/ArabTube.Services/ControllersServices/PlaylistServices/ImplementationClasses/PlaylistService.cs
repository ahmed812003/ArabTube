using ArabTube.Entities.DtoModels.PlaylistDTOs;
using ArabTube.Entities.Enums;
using ArabTube.Entities.GenericModels;
using ArabTube.Entities.Models;
using ArabTube.Entities.PlaylistModels;
using ArabTube.Services.ControllersServices.PlaylistServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ArabTube.Services.ControllersServices.PlaylistServices.ImplementationClasses
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public PlaylistService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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

            var playlistsDto = playlists.Select(p => new GetPlaylistDto
            {
                Id = p.Id,
                Title = p.Title,
                IsPrivate = p.IsPrivate
            });

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

            var playlistsDto = playlists.Select(p => new GetPlaylistDto
            {
                Id = p.Id,
                Title = p.Title,
                IsPrivate = p.IsPrivate
            });

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

            var playlistsDto = playlists.Select(p => new GetPlaylistDto
            {
                Id = p.Id,
                Title = p.Title,
                IsPrivate = p.IsPrivate
            });

            return new GetPlaylistsResult
            {
                IsSuccesed = true,
                Playlists = playlistsDto
            };
        }

        public async Task<ProcessResult> CreatePlaylistAsync(CreatePlaylistDto model, string userId)
        {
            var playlist = new Playlist
            {
                UserId = userId,
                Title = model.Title,
                Description = model.Description,
                IsPrivate = model.IsPrivate
            };

            var result = await _unitOfWork.Playlist.AddAsync(playlist);
            if (!result)
                return new ProcessResult { Message = "Error While Try To Create new Playlist" };
            await _unitOfWork.Complete();

            return new ProcessResult { IsSuccesed = true };
        }

        public async Task<ProcessResult> UpdatePlaylistAsync(UpdatePlaylistDto model)
        {
            var playlist = await _unitOfWork.Playlist.FindByIdAsync(model.PlaylistId);
            if (playlist == null)
            {
                return new ProcessResult { Message = $"No Playlist With Id = {model.PlaylistId}" };
            }

            if (playlist.IsDefult)
            {
                return new ProcessResult { Message = "Can't Update Defult Playlists" };
            }

            if (!string.IsNullOrEmpty(model.Title))
                playlist.Title = model.Title;

            if (!string.IsNullOrEmpty(model.Description))
                playlist.Title = model.Description;
            await _unitOfWork.Complete();

            return new ProcessResult { IsSuccesed = true };
        }

        public async Task<ProcessResult> DeletePlaylistAsync(string playlistId)
        {
            var playlist = await _unitOfWork.Playlist.FindByIdAsync(playlistId);
            if (playlist == null)
            {
                return new ProcessResult { Message = $"No Playlist With Id = {playlist}" };
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

        public async Task<string> GetPlaylistIdAsync(string playlistTitle, bool isDefaultPlaylist)
        {
            return await _unitOfWork.Playlist.FindPlaylistByNameAsync
                                              (PlaylistDefaultNames.PlaylistNames[0], true);
        }

    }
}
