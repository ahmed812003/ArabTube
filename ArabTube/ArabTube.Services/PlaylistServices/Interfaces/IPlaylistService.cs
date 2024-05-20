namespace ArabTube.Services.PlaylistServices.Interfaces
{
    public interface IPlaylistService
    {
        Task<bool> CheckDefultPlaylistsAsync(string userId);
        Task<bool> CreateDefaultPlaylists(string userId);
    }
}
