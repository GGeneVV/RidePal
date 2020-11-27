using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RidePal.Services.Contracts;
using RidePal.Web.Models;
using X.PagedList;

namespace RidePal.Web.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IArtistService _artistService;

        public ArtistsController(IMapper mapper, IArtistService artistService)
        {
            _mapper = mapper;
            
            _artistService = artistService;
        }
        // GET: ArtistsController
        public IActionResult Index(int? pageNumber = 1,
            string sortOrder = "",
            string searchString = "")
        {
            pageNumber = pageNumber ?? 1;
            var artistsVM = _artistService.GetAllArtists(pageNumber, sortOrder, searchString)
                .Select(g => _mapper.Map<ArtistVM>(g))
                .ToPagedList((int)pageNumber, 24);

            return View(artistsVM);
        }

        // GET: ArtistsController/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            var artist = await _artistService.GetArtistAsync(id);
            var artistVM = _mapper.Map<ArtistVM>(artist);

            return View(artistVM);
        }

        // GET: ArtistsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArtistsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ArtistsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ArtistsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ArtistsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ArtistsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
