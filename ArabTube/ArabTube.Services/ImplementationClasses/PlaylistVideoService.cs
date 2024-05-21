using ArabTube.Services.DataServices.Repositories.Interfaces;
using ArabTube.Services.Interfaces;

namespace ArabTube.Services.ImplementationClasses
{
    public class PlaylistVideoService : IPlaylistVideoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlaylistVideoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddVideoToPlayListAsync(string videoId, string playlistId)
        {
            var result = await _unitOfWork.PlaylistVideo.AddVideoToPlayListAsync(videoId, playlistId);
            if (!result)
            {
                return false;
            }
            return true;
        }
        
        public async Task<bool> FindVideoInPlaylistAsync(string videoId, string playlistId)
        {
            return await _unitOfWork.PlaylistVideo.FindVideoInPlaylist(videoId, playlistId);
        }
        
        public async Task<bool> RemoveVideoFromPlaylistAsync(string videoId, string playlistId)
        {
            var result = await FindVideoInPlaylistAsync(videoId, playlistId);
            if(!result)
            {
                return false;
            }
            result = await _unitOfWork.PlaylistVideo.RemoveVideoFromPlayListAsync(videoId, playlistId);
            if (!result)
            {
                return false;
            }
            return true;
        }
    }
}
