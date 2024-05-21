using ArabTube.Entities.DtoModels.PlaylistDTOs;

namespace ArabTube.Services.Interfaces
{
    public interface IPlaylistService
    {
        Task<bool> CheckDefultPlaylistsAsync(string userId);
        Task<bool> CreateDefaultPlaylists(string userId);
        Task<string> GetPlaylistId(string playlistTitle, bool isDefaultPlaylist);
        Task<IEnumerable<string>> SearchPlaylistTitlesAsync(string query);
        Task<IEnumerable<GetPlaylistDto>> SearchPlaylistAsync(string query);

    }
}
