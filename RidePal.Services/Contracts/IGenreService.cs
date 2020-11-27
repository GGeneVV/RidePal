using RidePal.Services.DTOModels;
using System;
using System.Linq;

namespace RidePal.Services.Contracts
{
    public interface IGenreService
    {
        GenreDTO GetGenreByIdAsync(Guid genreId);

        IQueryable<GenreDTO> GetAllGenres(int? pageNumber = 1,
            string sortOrder = "",
            string currentFilter = "",
            string searchString = "");
    }
}
