using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Services.Contracts
{
    public interface IArtistService
    {
        Task<ArtistDTO> GetArtistAsync(Guid id);

        IQueryable<ArtistDTO> GetAllArtists(
            int? pageNumber = 1,
            string sortOrder = "",
            string searchString = "");

        IReadOnlyCollection<ArtistDTO> GetTopArtists(int count = 5, string searchString = "");
    }
}
