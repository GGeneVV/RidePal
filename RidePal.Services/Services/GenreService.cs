using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Services.Contracts;
using RidePal.Services.DTOModels;
using RidePal.Services.Extensions;
using System;
using System.Linq;

namespace RidePal.Services
{
    public class GenreService : IGenreService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public GenreService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }


        public GenreDTO GetGenreByIdAsync(Guid genreId)
        {
            var genre = _appDbContext.Genres
                .AsNoTracking()
                .Where(g => g.Id == genreId)
                .FirstOrDefault(g => g.IsDeleted == false);

            var genreDTO = _mapper.Map<GenreDTO>(genre);

            return genreDTO;
        }

        public IQueryable<GenreDTO> GetAllGenres(
            int? pageNumber = 1,
            string sortOrder = "",
            string currentFilter = "",
            string searchString = "")
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            currentFilter = searchString;

            var genres = _appDbContext.Genres
                .Where(g => g.IsDeleted == false)
                .AsNoTracking()
                .WhereIf(!String.IsNullOrEmpty(searchString), s => s.Name.Contains(searchString))
                .Select(g => _mapper.Map<GenreDTO>(g));



            switch (sortOrder)
            {
                case "tracks_desc":
                    genres = genres.OrderByDescending(b => b.Tracks.Count);
                    break;
                case "Name":
                    genres = genres.OrderBy(b => b.Name);
                    break;
                case "Name_decs":
                    genres = genres.OrderByDescending(s => s.Name);
                    break;
                default:
                    genres = genres.OrderBy(s => s.Tracks.Count);
                    break;
            }

            return genres.AsQueryable();
        }
    }
}
