using RidePal.Models;
using RidePal.Services.DTOModels;
using RidePal.Services.DTOModels.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Services.Contracts
{
    public interface IPlaylistService
    {
        Task<PlaylistDTO> GeneratePlaylist(string from, string to, PlaylistConfigDTO playlistConfigDTO, Guid userId);
        IQueryable<PlaylistDTO> GetUserPlaylists(
            Guid userId,
            int? pageNumber = 1,
            string sortOrder = "",
            string searchString = "");
        Task<PlaylistDTO> GetPlaylist(Guid id);
        IQueryable<PlaylistDTO> GetAllPlaylists(int? pageNumber = 1,
            string sortOrder = "",
            string searchString = "");
        Task DeletePlaylist(Guid id);
        Task<PlaylistDTO> EditPlaylist(EditPlaylistDTO editPlaylistDTO);

        Task<int> GetTravelDurationAsync(string from, string to);

        IQueryable<PlaylistDTO> GetTopPlaylists(int count = 5);

    }
}
