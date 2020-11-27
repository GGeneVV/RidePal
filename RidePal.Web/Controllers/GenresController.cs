using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using RidePal.Services.Contracts;
using RidePal.Services.Pagination;
using RidePal.Web.Models;
using System;
using System.Linq;

namespace RidePal.Web.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly IMapper _mapper;
        private readonly ITrackService _trackService;

        public GenresController(IGenreService genreService, IMapper mapper, ITrackService trackService)
        {
            _genreService = genreService;
            _mapper = mapper;
            _trackService = trackService;
        }

        // GET: Genres
        [HttpGet("[controller]/{genreName}")]
        public IActionResult Index(int pageNumber = 1,
            string sortOrder = "",
            string searchString = "",
            string genreName = "")
        {
            var tracks = _trackService.GetAllTracks(sortOrder, searchString, genreName);

            var tracksVM = tracks.Select(t => _mapper.Map<TrackVM>(t)).ToPagedList(pageNumber, 24);
            ViewData["GenreName"] = genreName;
            return View(tracksVM);
            //int? pageNumber = 1,
            //string sortOrder = "",
            //string currentFilter = "",
            //string searchString = ""
            //var genresVM = _genreService.GetAllGenres(pageNumber, sortOrder, currentFilter, searchString)
            //    .Select(g => _mapper.Map<GenreVM>(g));

            //ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "tracks_desc" : "";
            //ViewData["DurationSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";

            //int pageSize = 10;
            //return View(PaginatedList<GenreVM>.Create(genresVM.AsQueryable(), pageNumber ?? 1, pageSize));
        }

        // GET: Genres/Details/5
        public IActionResult Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genreVM = _mapper.Map<GenreVM>(_genreService.GetGenreByIdAsync(id));

            if (genreVM == null)
            {
                return NotFound();
            }

            return View(genreVM);
        }


    }
}
