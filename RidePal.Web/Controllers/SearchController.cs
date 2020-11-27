using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RidePal.Services.Contracts;
using RidePal.Web.Models;
using System.Linq;

namespace RidePal.Web.Controllers
{
    [Route("search")]
    public class SearchController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGenreService _genreService;
        private readonly IArtistService _artistService;
        private readonly IAlbumService _albumService;
        private readonly ITrackService _trackService;
        private readonly IPlaylistService _playlistService;

        public SearchController(IMapper mapper, IGenreService genreService, IArtistService artistService, IAlbumService albumService, ITrackService trackService, IPlaylistService playlistService)
        {
            _mapper = mapper;
            _genreService = genreService;
            _artistService = artistService;
            _albumService = albumService;
            _trackService = trackService;
            _playlistService = playlistService;
        }

        // GET: Search
        //Browse all
        [HttpGet("Search")]
        public IActionResult Index()
        {
            var genres = _genreService.GetAllGenres();
            var genresVM = genres.Select(x => _mapper.Map<GenreVM>(x)).ToList();

            return View(genresVM);
        }

        [HttpGet("{searchString}")]
        public IActionResult BrowseAll(string searchString)
        {
            var tracks = _trackService.GetTopTracks(5, searchString: searchString);
            var tracksVM = tracks.Select(x => _mapper.Map<TrackVM>(x)).ToList();

            var artists = _artistService.GetTopArtists(6, searchString: searchString);
            var artistsVM = artists.Select(x => _mapper.Map<ArtistVM>(x)).ToList();

            var albums = _albumService.GetTopAlbums(6, searchString: searchString);
            var albumsVM = albums.Select(x => _mapper.Map<AlbumVM>(x)).ToList();

            //var playlists = _playlistService.Get

            if (albums.Count() == 0 && tracks.Count() == 0 && artists.Count == 0)
            {
                return NotFound();
            }

            var model = new BrowseAllVM()
            {
                Albums = albumsVM,
                Artists = artistsVM,
                //Playlists = playlistVM,
                Tracks = tracksVM,
            };

            ViewData["searchString"] = searchString;

            return View(model);
        }
    }
}
