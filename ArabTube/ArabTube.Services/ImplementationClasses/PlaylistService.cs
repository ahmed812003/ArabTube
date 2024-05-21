using ArabTube.Entities.DtoModels.PlaylistDTOs;
using ArabTube.Entities.Enums;
using ArabTube.Entities.Models;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using ArabTube.Services.Interfaces;

namespace ArabTube.Services.ImplementationClasses
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlaylistService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckDefultPlaylistsAsync(string userId)
        {
            return await _unitOfWork.Playlist.CheckDefultPlaylistsAsync(userId);
        }

        public async Task<bool> CreateDefaultPlaylists(string userId)
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
                    return false;
                }
                await _unitOfWork.Complete();
            }
            return true;
        }

        public async Task<string> GetPlaylistId(string playlistTitle, bool isDefaultPlaylist)
        {
           return await _unitOfWork.Playlist.FindPlaylistByNameAsync
                                             (PlaylistDefaultNames.PlaylistNames[0], true);

        }

        public Task<IEnumerable<GetPlaylistDto>> SearchPlaylistAsync(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> SearchPlaylistTitlesAsync(string query)
        {
            return await _unitOfWork.Playlist.SearchPlaylistTitlesAsync(query);
            
        }
    }
}
