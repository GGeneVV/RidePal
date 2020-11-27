using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Services.Contracts;
using RidePal.Services.DTOModels;
using RidePal.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Services
{
    public class ArtistService : IArtistService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public ArtistService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public IQueryable<ArtistDTO> GetAllArtists(
            int? pageNumber = 1,
            string sortOrder = "",
            string searchString = "")
        {
            sortOrder = sortOrder ?? "";
            searchString = searchString ?? "";

            var query = _appDbContext.Artists
                .AsNoTracking()
                .Where(t => t.IsDeleted == false);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Name.Contains(searchString));
            }
            
            switch (sortOrder)
            {
                case "ArtistsByAlbumsCount_desc":
                    query = query.OrderByDescending(b => b.Albums.Count);
                    break;
                case "Name":
                    query = query.OrderBy(b => b.Name);
                    break;
                case "Name_desc":
                    query = query.OrderByDescending(s => s.Name);
                    break;
                default:
                    query = query.OrderBy(s => s.Albums.Count);
                    break;
            }
            var artists = query.Select(artist => new ArtistDTO()
            {
                Id = artist.Id,
                Name = artist.Name,
                Picture = artist.Picture,
                Albums = artist.Albums.Select(x => _mapper.Map<AlbumDTO>(x)).ToList(),
                Tracks = artist.Tracks.Select(x => _mapper.Map<TrackDTO>(x)).ToList()
            });
            return artists;
        }

        public async Task<ArtistDTO> GetArtistAsync(Guid id)
        {
            if (id == null)

            {
                return null;
            }

            var artist = await _appDbContext.Artists
                .Where(x => x.IsDeleted == false)
                .Where(x => x.Id == id)
                .Select(artist => new ArtistDTO()
                    {
                    Id = artist.Id,
                    Name = artist.Name,
                    Picture = artist.Picture,
                    Albums = artist.Albums.Select(x => _mapper.Map<AlbumDTO>(x)).ToList(),
                    Tracks = artist.Tracks.Select(x => _mapper.Map<TrackDTO>(x)).ToList()
                }).FirstOrDefaultAsync();

            if (artist == null)
            {
                return null;
            }

            return artist;
        }

        public IReadOnlyCollection<ArtistDTO> GetTopArtists(int count = 5, string searchString = "")
        {
            IReadOnlyCollection<ArtistDTO> artists = new List<ArtistDTO>();
            var query = _appDbContext.Artists
                .AsNoTracking()
                .Where(x => x.IsDeleted == false);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
            }
            artists = query
                .OrderByDescending(x => x.Tracks.Count)
                .Take(count)
                .Include(x => x.Albums)
                .Include(x => x.Tracks)
                .Select(a => _mapper.Map<ArtistDTO>(a))
                .ToList();


            return artists;
        }
    }
}
