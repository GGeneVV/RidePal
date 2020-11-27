using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using RidePal.Services.Contracts;
using RidePal.Web.Models;
using System;
using System.Linq;

namespace RidePal.Web.Controllers
{
    public class TracksController : Controller
    {
        private readonly ITrackService _trackService;
        private readonly IMapper _mapper;

        public TracksController(ITrackService trackService, IMapper mapper)
        {
            _trackService = trackService;
            _mapper = mapper;
        }

        // GET: Tracks
        public IActionResult Index(int pageNumber = 1, string sortOrder = "", string searchString = "")
        {
            int pageSize = 15;

            var tracks = _trackService.GetAllTracks(sortOrder, searchString);
            var tracksVM = tracks.Select(x => _mapper.Map<TrackVM>(x));

            var tracksList = tracksVM.ToPagedList(pageNumber, pageSize);

            return View(tracksList);
        }

        // GET: Tracks/Details/5
        public IActionResult Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trackVM = _mapper.Map<TrackVM>(_trackService.GetTrackAsync(id));

            if (trackVM == null)
            {
                return NotFound();
            }

            return View(trackVM);
        }
    }
}
