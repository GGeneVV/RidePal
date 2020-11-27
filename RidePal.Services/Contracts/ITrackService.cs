using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Services.Contracts
{
    public interface ITrackService
    {
        Task<TrackDTO> GetTrackAsync(Guid id);
        IQueryable<TrackDTO> GetAllTracks(
            string sortOrder = "",
            string searchString = "",
            string genreString = "",
            string artistString = "");


        IQueryable<TrackDTO> GetPopularTracks(int count = 5);
        IQueryable<TrackDTO> GetTopTracks(int count = 5, string searchString = "");
    }
}
