using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using RidePal.Services.Contracts;
using RidePal.Services.Pagination;
using RidePal.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Web.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly IMapper _mapper;

        public AlbumsController(IAlbumService albumService, IMapper mapper)
        {
            _albumService = albumService;
            _mapper = mapper;
        }

        // GET: Albums
        public IActionResult Index(int? pageNumber = 1,
            string sortOrder = "",
            string searchString = "")
        {
            pageNumber = pageNumber ?? 1;
            var albumsVM = _albumService.GetAllAlbums(pageNumber, sortOrder, searchString)
                .Select(g => _mapper.Map<AlbumVM>(g))
                .ToPagedList((int)pageNumber, 24);

            return View(albumsVM);
        }

        public async Task<IActionResult> NowListening(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var album = await _albumService.GetAlbumByIdAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            var albumVM = _mapper.Map<AlbumVM>(album);
            return View(albumVM);
        }
    }
}
