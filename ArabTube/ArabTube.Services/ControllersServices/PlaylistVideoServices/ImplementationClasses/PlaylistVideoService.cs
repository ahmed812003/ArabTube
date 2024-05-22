using ArabTube.Entities.DtoModels.PlaylistDTOs;
using ArabTube.Entities.GenericModels;
using ArabTube.Entities.PlaylistModels;
using ArabTube.Services.ControllersServices.PlaylistVideoServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using AutoMapper;

namespace ArabTube.Services.ControllersServices.PlaylistVideoServices.ImplementationClasses
{
    public class PlaylistVideoService : IPlaylistVideoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlaylistVideoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProcessResult> AddVideoToPlayListAsync(string videoId, string playlistId)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(videoId);
            if (video == null)
            {
                return new ProcessResult { Message = $"No Video With Id = {videoId}" };
            }

            var playlist = await _unitOfWork.Playlist.FindByIdAsync(playlistId);
            if (playlist == null)
            {
                return new ProcessResult { Message = $"No Playlist With Id = {playlistId}" };
            }

            var result = await _unitOfWork.PlaylistVideo.AddVideoToPlayListAsync(videoId, playlistId);
            if (!result)
            {
                return new ProcessResult { IsSuccesed = true, Message = "The Video Is Already In The Playlist" };
            }

            await _unitOfWork.Complete();
            return new ProcessResult { IsSuccesed = true, Message = "Video Added Succesfully" };
        }

        public async Task<GetPlaylistVideosResult> GetPlaylistVideosAsync(string playlistId)
        {
            var playlist = await _unitOfWork.Playlist.FindByIdAsync(playlistId);
            if (playlist == null)
            {
                return new GetPlaylistVideosResult { Message = $"No Playlist With Id = {playlistId}" };
            }

            var playlistVideos = await _unitOfWork.PlaylistVideo.GetPlaylistVideosAsync(playlistId);

            if (!playlistVideos.Any())
                return new GetPlaylistVideosResult { Message = $"The Playlist With Id = {playlistId} Dosn't Has Any Videos" };

            var videos = _mapper.Map<IEnumerable<PlaylistVideoDto>>(playlistVideos);

            return new GetPlaylistVideosResult
            {
                IsSuccesed = true,
                Videos = videos
            };
        }

        public async Task<bool> FindVideoInPlaylistAsync(string videoId, string playlistId)
        {
            return await _unitOfWork.PlaylistVideo.FindVideoInPlaylistAsync(videoId, playlistId);
        }

        public async Task<ProcessResult> RemoveVideoFromPlaylistAsync(string videoId, string playlistId)
        {
            var video = await _unitOfWork.Video.FindByIdAsync(videoId);
            if (video == null)
            {
                return new ProcessResult { Message = $"No Video With Id = {videoId}" };
            }

            var playlist = await _unitOfWork.Playlist.FindByIdAsync(playlistId);
            if (playlist == null)
            {
                return new ProcessResult { Message = $"No Playlist With Id = {playlistId}" };
            }

            var result = await _unitOfWork.PlaylistVideo.RemoveVideoFromPlayListAsync(videoId, playlistId);
            if (!result)
            {
                return new ProcessResult { Message = "The Video Is Already Not In The Playlist" };
            }

            await _unitOfWork.Complete();
            return new ProcessResult { IsSuccesed = true };
        }
    }
}
