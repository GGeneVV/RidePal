using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Services.Contracts
{
    public interface IAlbumService
    {
        Task<AlbumDTO> GetAlbumByIdAsync(Guid albumId);

        IQueryable<AlbumDTO> GetAllAlbums(
            int? pageNumber = 1,
            string sortOrder = "",
            string searchString = "");

        IQueryable<AlbumDTO> GetTopAlbums(int count = 5, string searchString = "");

    }
}
