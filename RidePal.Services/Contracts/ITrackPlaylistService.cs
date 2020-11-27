using RidePal.Services.DTOModels;
using RidePal.Services.Pagination;
using System;

namespace RidePal.Services.Contracts
{
    public interface ITrackPlaylistService
    {
        TrackPlaylistDTO GetTrackPlaylistByIdAsync(Guid trackPlaylistId);

        PaginatedList<TrackPlaylistDTO> GetAllTrackPlaylistsAsync();
    }
}
