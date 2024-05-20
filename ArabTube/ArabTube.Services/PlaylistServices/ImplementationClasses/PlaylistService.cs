using ArabTube.Entities.Enums;
using ArabTube.Entities.Models;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using ArabTube.Services.PlaylistServices.Interfaces;

namespace ArabTube.Services.PlaylistServices.ImplementationClasses
{
    public class PlaylistService:IPlaylistService
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
    }
}
