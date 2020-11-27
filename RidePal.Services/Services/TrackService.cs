using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Services.Contracts;
using RidePal.Services.DTOMappers;
using RidePal.Services.DTOModels;
using RidePal.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Services
{
    public class TrackService : ITrackService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public TrackService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public IQueryable<TrackDTO> GetAllTracks(
            string sortOrder = "",
            string searchString = "",
            string genreString = "",
            string artistString = "")
        {
            sortOrder = sortOrder ?? "";
            searchString = searchString ?? "";
            genreString = genreString ?? "";
            artistString = artistString ?? "";

            var query = _appDbContext.Tracks
                .AsNoTracking()
                .Where(t => t.IsDeleted == false);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Title.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(genreString))
            {
                query = query.Where(s => s.Genre.Name.Equals(genreString));
            }
            if (!string.IsNullOrEmpty(artistString))
            {
                query = query.Where(s => s.Artist.Name.Equals(artistString));
            }


            switch (sortOrder.ToLower())
            {
                case "rank_desc":
                    query = query.OrderByDescending(b => b.Rank);
                    break;
                case "title":
                    query = query.OrderBy(b => b.Title);
                    break;
                case "title_desc":
                    query = query.OrderByDescending(s => s.Title);
                    break;
                case "artist":
                    query = query.OrderBy(b => b.Artist.Name);
                    break;
                case "artist_decs":
                    query = query.OrderByDescending(s => s.Artist.Name);
                    break;
                case "duration":
                    query = query.OrderBy(b => b.Duration);
                    break;
                case "duration_desc":
                    query = query.OrderByDescending(s => s.Duration);
                    break;
                case "release":
                    query = query.OrderBy(b => b.ReleaseDate);
                    break;
                case "release_desc":
                    query = query.OrderByDescending(s => s.ReleaseDate);
                    break;
                default:
                    query = query.OrderBy(s => s.Rank);
                    break;
            }


            //Projection Loading
            var tracks = query.Select(track => new TrackDTO()
            {
                Id = track.Id,
                DeezerId = track.DeezerId,
                Title = track.Title,
                SongURL = track.SongURL,
                Duration = track.Duration,
                Rank = track.Rank,
                ReleaseDate = track.ReleaseDate,
                Preview = track.Preview,
                Album = _mapper.Map<AlbumDTO>(track.Album),
                AlbumId = track.AlbumId,
                Artist = _mapper.Map<ArtistDTO>(track.Artist),
                ArtistId = track.ArtistId,
                Genre = _mapper.Map<GenreDTO>(track.Genre),
                GenreId = track.GenreId,
            });
            return tracks;

        }

        public async Task<TrackDTO> GetTrackAsync(Guid id)
        {
            if (id == null)
                throw new ArgumentNullException();

            var track = await _appDbContext.Tracks
                .Where(x => x.IsDeleted == false)
                .Where(x => x.Id == id)
                .Include(t => t.Album)
                .Include(t => t.Artist)
                .Include(t => t.Genre)
                .Include(t => t.TrackPlaylists)
                .FirstOrDefaultAsync();

            if (track == null)
                throw new ArgumentNullException();

            var trackDTO = _mapper.Map<TrackDTO>(track);

            return trackDTO;
        }

        public IQueryable<TrackDTO> GetPopularTracks(int count = 5)
        {
            var tracks = _appDbContext.Tracks
                .AsNoTracking()
                .Where(t => t.IsDeleted == false)
                .OrderByDescending(t => t.ReleaseDate)
                .Take(count)
                //Projection Loading
                .Select(track => new TrackDTO()
                {
                    Id = track.Id,
                    DeezerId = track.DeezerId,
                    Title = track.Title,
                    SongURL = track.SongURL,
                    Duration = track.Duration,
                    Rank = track.Rank,
                    ReleaseDate = track.ReleaseDate,
                    Preview = track.Preview,
                    Album = _mapper.Map<AlbumDTO>(track.Album),
                    AlbumId = track.AlbumId,
                    Artist = _mapper.Map<ArtistDTO>(track.Artist),
                    ArtistId = track.ArtistId,
                    Genre = _mapper.Map<GenreDTO>(track.Genre),
                    GenreId = track.GenreId,
                });

            return tracks;
        }

        public IQueryable<TrackDTO> GetTopTracks(int count = 5, string searchString = "")
        {
            var query = _appDbContext.Tracks
                .AsNoTracking()
                .Where(t => t.IsDeleted == false);
                

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.Title.ToLower().Contains(searchString.ToLower()));
            }
            var tracks = query
                .OrderByDescending(x => x.Rank)
                .Take(count)
                //Projection Loading
                .Select(track => new TrackDTO()
                {
                    Id = track.Id,
                    DeezerId = track.DeezerId,
                    Title = track.Title,
                    SongURL = track.SongURL,
                    Duration = track.Duration,
                    Rank = track.Rank,
                    ReleaseDate = track.ReleaseDate,
                    Preview = track.Preview,
                    Album = _mapper.Map<AlbumDTO>(track.Album),
                    AlbumId = track.AlbumId,
                    Artist = _mapper.Map<ArtistDTO>(track.Artist),
                    ArtistId = track.ArtistId,
                    Genre = _mapper.Map<GenreDTO>(track.Genre),
                    GenreId = track.GenreId,
                });

            return tracks;
        }
    }
}
