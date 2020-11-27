using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RidePal.Services.Contracts;
using RidePal.Web.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenreService _genreService;
        private readonly IPlaylistService _playlistService;
        private readonly ITrackService _trackService;
        private readonly IArtistService _artistService;
        private readonly IAlbumService _albumService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IGenreService genreService, IPlaylistService playlistService, ITrackService trackService, IArtistService artistService, IAlbumService albumService, IMapper mapper)
        {
            _logger = logger;
            _genreService = genreService;
            _playlistService = playlistService;
            _trackService = trackService;
            _artistService = artistService;
            _albumService = albumService;
            _mapper = mapper;


        }


        public IActionResult Index([Bind("Title,UseTopTracks,AllowTracksFromSameArtist,IsAdvanced")] PlaylistConfigVM playlistConfigVM,
            [Bind("IsChecked,Percentage")] GenreConfigVM genreVM)
        {
            var genres = _genreService.GetAllGenres();
            var genreConfigs = genres.Select(g => _mapper.Map<GenreConfigVM>(g)).ToList();
            var popularTracksVM = _trackService.GetPopularTracks(5)
                .Select(t => _mapper.Map<TrackVM>(t))
                .AsEnumerable();

            var topTracksVM = _trackService.GetTopTracks(6)
                .Select(t => _mapper.Map<TrackVM>(t))
                .AsEnumerable();

            var topArtistsVM = _artistService.GetTopArtists(6)
                .Select(t => _mapper.Map<ArtistVM>(t))
                .AsEnumerable();

            var topAlbumsVM = _albumService.GetTopAlbums(6)
                .Select(t => _mapper.Map<AlbumVM>(t))
                .AsEnumerable();

            var topPlaylistsVM = _playlistService.GetTopPlaylists(6)
                .Select(p => _mapper.Map<PlaylistVM>(p))
                .AsEnumerable();

            var playlistConfig = new PlaylistConfigVM()
            {
                Title = playlistConfigVM.Title,
                UseTopTracks = playlistConfigVM.UseTopTracks,
                IsAdvanced = playlistConfigVM.IsAdvanced,
                GenreConfigs = genreConfigs
            };

            var model = new HomeIndexVM()
            {
                PlaylistConfig = playlistConfig,
                PopularTracks = popularTracksVM,
                TopTracks = topTracksVM,
                TopArtists = topArtistsVM,
                TopAlbums = topAlbumsVM,
                TopPlaylists = topPlaylistsVM,
            };


            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
