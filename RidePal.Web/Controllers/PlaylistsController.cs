using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RidePal.Services.Contracts;
using RidePal.Services.DTOModels;
using RidePal.Services.DTOModels.Configurations;
using RidePal.Services.Pagination;
using RidePal.Services.Wrappers.Contracts;
using RidePal.Web.Models;
using RidePal.Web.Models.EditVM;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using X.PagedList;

namespace RidePal.Web.Controllers
{

    public class PlaylistsController : Controller
    {

        private readonly IPlaylistService _playlistService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUserManagerWrapper _userManagerWrapper;

        public PlaylistsController(IPlaylistService playlistService, IMapper mapper, IUserService userService,
            IUserManagerWrapper userManagerWrapper)
        {

            _playlistService = playlistService;
            _mapper = mapper;
            _userService = userService;
            _userManagerWrapper = userManagerWrapper;
        }

        // GET: Playlists
        public IActionResult Index(
            string sortOrder = "",
            string searchString = "",
            int? pageNumber = 1)
        {
            pageNumber = pageNumber ?? 24;
            int pageSize = 24;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "rank_desc" : "";
            ViewData["DurationSortParm"] = sortOrder == "Duration" ? "duration_desc" : "Duration";

            var playlistsVM = _playlistService
                 .GetAllPlaylists(pageNumber, sortOrder, searchString)
                 .Select(p => _mapper.Map<PlaylistVM>(p))
                 .ToPagedList((int)pageNumber, pageSize);

            return View(playlistsVM);
        }
        [HttpGet]
        public IActionResult MyPlaylists(
            int? pageNumber = 1,
            string sortOrder = "",
            string currentFilter = "",
            string searchString = "")
        {
            var userId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var playlistst = _playlistService.GetUserPlaylists(userId, pageNumber, sortOrder, searchString)
                .Select(p => _mapper.Map<PlaylistVM>(p));

            int pageSize = 5;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "tracks_desc" : "";
            ViewData["DurationSortParm"] = sortOrder == "Duration" ? "duration_desc" : "Duration";

            return View(PaginatedList<PlaylistVM>.Create(playlistst.AsQueryable(), pageNumber ?? 1, pageSize));
        }

        // GET: Playlists/Details/5
        public IActionResult Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistVM = _mapper.Map<PlaylistVM>(_playlistService.GetPlaylist(id));

            if (playlistVM == null)
            {
                return NotFound();
            }

            return View(playlistVM);
        }

        // GET: Playlists/Create


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GeneratePlaylist(string from, string to, PlaylistConfigVM playlistConfig)
        {

            var user = await _userManagerWrapper.GetUserAsync(User);

            var dto = _mapper.Map<PlaylistConfigDTO>(playlistConfig);
            var playlistDTO = await _playlistService.GeneratePlaylist(from, to, dto, user.Id);
            var playlistId = playlistDTO.Id;

            return RedirectToAction("NowListening", new { id = playlistId });

        }

        public async Task<IActionResult> NowListening(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var playlist = await _playlistService.GetPlaylist(id);
            if (playlist == null)
            {
                return NotFound();
            }
            var playlistVM = _mapper.Map<PlaylistVM>(playlist);
            return View(playlistVM);
        }


        public async Task<IActionResult> UserPlaylists(int pageNumber = 1, string sortOrder = "")
        {
            var user = await _userManagerWrapper.GetUserAsync(User);
            var playlist = _playlistService.GetUserPlaylists(user.Id, sortOrder: sortOrder);
            if (playlist == null)
            {
                return NotFound();
            }
            int pageSize = 24;
            var playlistVM = playlist.Select(p => _mapper.Map<PlaylistVM>(p)).ToPagedList(pageNumber, pageSize);

            return View(playlistVM);
        }

        // GET: Playlists/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlist = await _playlistService.GetPlaylist(id);

            if (playlist == null)
            {
                return NotFound();
            }


            return View(playlist);
        }

        // POST: Playlists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("Id,UserId,Revive")] EditPlaylistVM editPlaylistVM)
        {

            try
            {
                var editPlaylistDTO = _mapper.Map<EditPlaylistDTO>(editPlaylistVM);
                var playlist = await _playlistService.EditPlaylist(editPlaylistDTO);
                return View(playlist);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // GET: Playlists/Delete/5
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlist = _playlistService.GetPlaylist(id);

            if (playlist == null)
            {
                return NotFound();
            }

            return View(playlist);
        }

        // POST: Playlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var playlist = _playlistService.DeletePlaylist(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
