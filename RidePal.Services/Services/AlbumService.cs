using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Models;
using RidePal.Services.Contracts;
using RidePal.Services.DTOModels;
using RidePal.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public AlbumService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<AlbumDTO> GetAlbumByIdAsync(Guid albumId)
        {
            var album = await _appDbContext.Albums
                .AsNoTracking()
                .Where(a => a.Id == albumId)
                .Include(t => t.Tracks)
                    .ThenInclude(t => t.Genre)
                .Include(t => t.Tracks)
                    .ThenInclude(t => t.Artist)
                .FirstOrDefaultAsync(a => a.IsDeleted == false);

            var albumDTO = _mapper.Map<AlbumDTO>(album);

            return albumDTO;
        }

        public IQueryable<AlbumDTO> GetAllAlbums(
             int? pageNumber = 1,
            string sortOrder = "",
            string searchString = "")
        {
            var query = _appDbContext.Albums
                .AsNoTracking()
                .Where(a => a.IsDeleted == false);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(a => a.Title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    query = query.OrderByDescending(a => a.Title);
                    break;
                case "NameOfArtist":
                    query = query.OrderBy(a => a.Artist.Name);
                    break;
                case "NameOfArtist_desc":
                    query = query.OrderByDescending(a => a.Artist.Name);
                    break;
                default:
                    query = query.OrderBy(a => a.Title);
                    break;
            }
            var albums = query.Select(a => new AlbumDTO()
            {
                Artist = _mapper.Map<ArtistDTO>(a.Artist),
                ArtistId = a.ArtistId,
                Id = a.Id,
                Picture = a.Picture,
                Title = a.Title,
                Tracklist = a.Tracklist,
                Tracks = a.Tracks.Select(x => _mapper.Map<TrackDTO>(x)).ToList()
            });

            return albums.AsQueryable();
        }

        public IQueryable<AlbumDTO> GetTopAlbums(int count = 5, string searchString = "")
        {
            var query = _appDbContext.Albums
                .AsNoTracking()
                .Where(x => x.IsDeleted == false);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.Title.ToLower().Contains(searchString.ToLower()));
            }
            var albums = query
                .OrderByDescending(x => x.Tracks.Count)
                .Take(count)
                .Select(a => new AlbumDTO(){
                    Artist = _mapper.Map<ArtistDTO>(a.Artist),
                    ArtistId = a.ArtistId,
                    Id = a.Id,
                    Picture = a.Picture,
                    Title = a.Title,
                    Tracklist = a.Tracklist,
                    Tracks = a.Tracks.Select(x => _mapper.Map<TrackDTO>(x)).ToList()
                });

            return albums;
        }
    }
}
