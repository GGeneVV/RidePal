using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RidePal.Data;
using RidePal.Models;
using RidePal.Services.Contracts;
using RidePal.Services.DTOModels;
using RidePal.Services.DTOModels.Configurations;
using RidePal.Services.Extensions;
using RidePal.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RidePal.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUserManagerWrapper _userManagerWrapper;

        public PlaylistService(AppDbContext appDbContext, IMapper mapper,
            IUserManagerWrapper userManagerWrapper,
            IUserService userService
            )
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _userManagerWrapper = userManagerWrapper;
            _userService = userService;

        }

        private async Task<IReadOnlyCollection<Track>> RandomTracksByGenreConfig(PlaylistConfigDTO playlistConfigDTO, string genreName)
        {
            var genreNames = playlistConfigDTO.GenreConfigs
                .Where(g => g.IsChecked == true)
                .Select(g => g.Name);

            var tracks = await _appDbContext.Tracks
                .AsNoTracking()
                .Where(t => t.IsDeleted == false)
                .Include(g => g.Genre)
                .Where(t => genreNames.Contains(t.Genre.Name) && t.Genre.Name.Equals(genreName))
                .OrderBy(t => Guid.NewGuid())
                .ToListAsync();

            return tracks;
        }

        public async Task<int> GetTravelDurationAsync(string from, string to)
        {
            var client = new HttpClient();
            using (var response = await client.GetAsync($"https://dev.virtualearth.net/REST/V1/Routes/Driving?wp.0={from}&wp.1={to}&key=Ao0FujyUA1avU6phfkgErqA_GnmNs26KUgvWt6v_HKDXM3wEpZKzn_8j2-LToLbM"))
            {
                var responseAsString = await response.Content.ReadAsStringAsync();
                var res = JObject.Parse(responseAsString)["resourceSets"][0]["resources"][0]["travelDuration"];
                int travelDuration;
                if (!int.TryParse(res.ToString(), out travelDuration))
                {
                    return 0;
                }
                return travelDuration;
            }
        }

        public async Task<PlaylistDTO> GeneratePlaylist(string from, string to, PlaylistConfigDTO playlistConfigDTO, Guid userId)
        {
            var travelDuration = await GetTravelDurationAsync(from, to);

            if (travelDuration <= 0 || playlistConfigDTO == null)
            {
                return null;
            }

            int totalDuration = 0;

            var genres = playlistConfigDTO.GenreConfigs
                .Where(x => x.IsChecked == true); // Get only checked genres

            int genresCount = genres.Count();
            if (genresCount <= 0)
            {
                return null;
            }
            int avgMinPerGenre = 300 / genresCount; // Get Average additional minute per genre ( 5min MAX )

            //Initialize playlist
            var playlist = new Playlist()
            {
                CreatedOn = DateTime.Now,
                Title = playlistConfigDTO.Title,
                UserId = userId,
            };

            ICollection<TrackPlaylist> trackPlaylist = new List<TrackPlaylist>();

            foreach (var genre in genres)
            {
                int durationPerGenre = 0;
                int genreDuration = 0;
                var genreTracks = await RandomTracksByGenreConfig(playlistConfigDTO, genre.Name);

                if (genre.Percentage <= 0 && playlistConfigDTO.IsAdvanced == false)
                {
                    double genrePercent = 100 / genresCount;
                    durationPerGenre = (int)Math.Floor(travelDuration * genrePercent) / 100;
                }
                else
                {
                    durationPerGenre = (travelDuration * genre.Percentage) / 100;
                }

                int count = 0;
                while (true)
                {
                    if (count >= genreTracks.Count()) { break; }

                    //Gets Element at index 
                    var track = genreTracks.ElementAt(count);

                    //Break if next track will surpass Max duation per genre
                    if (genreDuration + track.Duration > durationPerGenre + avgMinPerGenre) { break; }

                    totalDuration += track.Duration;
                    genreDuration += track.Duration;
                    trackPlaylist.Add(new TrackPlaylist()
                    {
                        CreatedOn = DateTime.Now,
                        PlaylistId = playlist.Id,
                        Playlist = playlist,
                        TrackId = track.Id,
                        Track = track,
                    });

                    count++;
                }
            }
            trackPlaylist = ArrangePlaylist(playlistConfigDTO, trackPlaylist);

            playlist.Rank = (int)trackPlaylist.Average(t => t.Track.Rank);
            playlist.Duration = totalDuration;

            var trackPlaylistDB = trackPlaylist
                .Select(x => new TrackPlaylist()
                {
                    PlaylistId = x.PlaylistId,
                    TrackId = x.TrackId,
                })
                .ToList();

            playlist.TrackPlaylists = trackPlaylistDB;
            await _appDbContext.Playlists.AddAsync(playlist);
            await _appDbContext.SaveChangesAsync();

            playlist.TrackPlaylists = trackPlaylist;

            var playlistDTO = _mapper.Map<PlaylistDTO>(playlist);

            return playlistDTO;
        }

        private static ICollection<TrackPlaylist> ArrangePlaylist(PlaylistConfigDTO playlistConfigDTO, ICollection<TrackPlaylist> trackPlaylist)
        {
            if (playlistConfigDTO.UseTopTracks)
            {
                trackPlaylist = trackPlaylist.OrderBy(t => t.Track.Rank).ToList();
            }
            else
            {
                trackPlaylist = trackPlaylist.OrderByDescending(t => t.Track.Rank).ToList();
            }

            return trackPlaylist;
        }

        public IQueryable<PlaylistDTO> GetUserPlaylists(
            Guid userId,
            int? pageNumber = 1,
            string sortOrder = "",
            string searchString = "")
        {
            if (userId == null)
            {
                return null;
            }

            var query = _appDbContext.Playlists
                .Where(p => p.UserId == userId && p.IsDeleted == false);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query
                .Where(s => s.Title.Contains(searchString));
            }

            var playlists = query
                .Select(p => new PlaylistDTO()
                {
                    Title = p.Title,
                    Duration = p.Duration,
                    Enabled = p.Enabled,
                    Id = p.Id,
                    IsArtistRepeated = p.IsArtistRepeated,
                    IsTopTracksEnabled = p.IsTopTracksEnabled,
                    Picture = p.Picture,
                    Rank = p.Rank,
                    TrackPlaylists = p.TrackPlaylists.Select(t => new TrackPlaylistDTO()
                    {
                        Track = new TrackDTO()
                        {
                            Artist = _mapper.Map<ArtistDTO>(t.Track.Artist)
                        },
                        TrackId = t.TrackId
                    }).ToList(),
                    User = _mapper.Map<UserDTO>(p.User),
                    UserId = p.UserId
                });

            switch (sortOrder)
            {
                case "rank_desc":
                    playlists = playlists.OrderBy(b => b.Rank);
                    break;
                case "Duration":
                    playlists = playlists.OrderBy(b => b.Duration);
                    break;
                case "Duration_decs":
                    playlists = playlists.OrderByDescending(s => s.Duration);
                    break;
                default:
                    playlists = playlists.OrderByDescending(s => s.Rank);
                    break;
            }

            return playlists;
        }

        public async Task<PlaylistDTO> GetPlaylist(Guid id)
        {
            if (id == null)
            {
                return null;
            }

            var playlist = await _appDbContext.Playlists
                .Where(p => p.IsDeleted == false && p.Id == id)
                .Include(t => t.TrackPlaylists)
                    .ThenInclude(t => t.Track)
                        .ThenInclude(t => t.Artist)
                .Include(t => t.TrackPlaylists)
                    .ThenInclude(t => t.Track)
                        .ThenInclude(t => t.Album)
                .Include(t => t.TrackPlaylists)
                    .ThenInclude(t => t.Track)
                        .ThenInclude(t => t.Genre)
                .FirstOrDefaultAsync();

            if (playlist == null)
            {
                return null;
            }

            var dto = _mapper.Map<PlaylistDTO>(playlist);

            return dto;
        }

        public IQueryable<PlaylistDTO> GetAllPlaylists(
            int? pageNumber = 1,
            string sortOrder = "",
            string searchString = "")
        {

            var query = _appDbContext.Playlists
                .Where(p => p.IsDeleted == false);
                if (!string.IsNullOrEmpty(searchString))
                {
                    query = query.Where(s => s.Title.Contains(searchString));
                }
                var playlists = query
                .Select(p => new PlaylistDTO()
                {
                    Title = p.Title,
                    Duration = p.Duration,
                    Enabled = p.Enabled,
                    Id = p.Id,
                    IsArtistRepeated = p.IsArtistRepeated,
                    IsTopTracksEnabled = p.IsTopTracksEnabled,
                    Picture = p.Picture,
                    Rank = p.Rank,
                    TrackPlaylists = p.TrackPlaylists.Select(t => new TrackPlaylistDTO()
                    {
                        Track = new TrackDTO()
                        {
                            Artist = _mapper.Map<ArtistDTO>(t.Track.Artist)
                        },
                        TrackId = t.TrackId
                    }).ToList(),
                    User = _mapper.Map<UserDTO>(p.User),
                    UserId = p.UserId
                });

            switch (sortOrder)
            {
                case "rank_desc":
                    playlists = playlists.OrderByDescending(b => b.Rank);
                    break;
                case "Duration":
                    playlists = playlists.OrderBy(b => b.Duration);
                    break;
                case "Duration_decs":
                    playlists = playlists.OrderByDescending(s => s.Duration);
                    break;
                default:
                    playlists = playlists.OrderBy(s => s.Rank);
                    break;
            }

            return playlists;
        }

        public async Task DeletePlaylist(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException();
            }

            var playlist = await _appDbContext.Playlists
                .Where(p => p.IsDeleted == false)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (playlist == null)
            {
                throw new ArgumentNullException();
            }

            playlist.IsDeleted = true;
            _appDbContext.Playlists.Update(playlist);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<PlaylistDTO> EditPlaylist(EditPlaylistDTO editPlaylistDTO)
        {
            if (editPlaylistDTO.Id == null) { throw new ArgumentNullException(); }

            var playlist = await _appDbContext.Playlists
                .Include(t => t.TrackPlaylists)
                    .ThenInclude(t => t.Track)
                .Where(p => p.IsDeleted == false || editPlaylistDTO.Revive == true)
                .FirstOrDefaultAsync(p => p.Id == editPlaylistDTO.Id);

            if (playlist == null) { throw new ArgumentNullException(); }
            if (editPlaylistDTO.Revive == true)
            {
                playlist.IsDeleted = false;
            }
            playlist.Title = editPlaylistDTO.Title;
            _appDbContext.Update(playlist);
            await _appDbContext.SaveChangesAsync();
            return _mapper.Map<PlaylistDTO>(playlist);
        }

        public IQueryable<PlaylistDTO> GetTopPlaylists(int count = 5)
        {
            if (count == 0)
            {
                return null;
            }

            var playlists = _appDbContext.Playlists
                .Where(p => p.IsDeleted == false)
                .Select(p => new PlaylistDTO()
                {
                    Title = p.Title,
                    Duration = p.Duration,
                    Enabled = p.Enabled,
                    Id = p.Id,
                    IsArtistRepeated = p.IsArtistRepeated,
                    IsTopTracksEnabled = p.IsTopTracksEnabled,
                    Picture = p.Picture,
                    Rank = p.Rank,
                    TrackPlaylists = p.TrackPlaylists.Select(t => new TrackPlaylistDTO()
                    {
                        Track = new TrackDTO()
                        {
                            Artist = _mapper.Map<ArtistDTO>(t.Track.Artist)
                        },
                        TrackId = t.TrackId
                    }).ToList(),
                    User = _mapper.Map<UserDTO>(p.User),
                    UserId = p.UserId
                });

            return playlists;
        }
    }
}
