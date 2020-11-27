using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RidePal.Data;
using RidePal.Models;
using RidePal.Services.DTOMappers;
using System;

namespace Ridepal.Tests
{
    public class Utils
    {

        public static IMapper Mapper { get => new Mapper(new MapperConfiguration(conf => conf.AddProfile(new DTOMapperProflie()))); }

        public static DbContextOptions<AppDbContext> GetOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        public static Album CreateMockAlbum(string title, int deezerId, Guid artistId)
        {
            return new Album
            {
                Id = Guid.NewGuid(),
                DeezerId = deezerId.ToString(),
                Title = title,
                Tracklist = $"https://api.deezer.com/album/{deezerId.ToString()}/tracks",
                ArtistId = artistId,
            };
        }
        public static Artist CreateMockArtist(string name, int deezerId)
        {
            return new Artist
            {
                Id = Guid.NewGuid(),
                DeezerId = deezerId.ToString(),
                Name = name,
                Picture = $"https://api.deezer.com/artist/{deezerId.ToString()}/image",
                TrackListURL = $"https://api.deezer.com/artist/{deezerId.ToString()}/top?limit=50",
            };
        }
        public static Genre CreateMockGenre(string name, int deezerId)
        {
            return new Genre
            {
                Id = Guid.NewGuid(),
                DeezerId = deezerId.ToString(),
                Name = name,
                Picture = $"https://api.deezer.com/genre/{deezerId.ToString()}/image",
            };
        }
        public static Playlist CreateMockPlaylist(string title, Guid userId, int duration = 1000, int rank = 10000, bool isDeleted = false)
        {
            return new Playlist
            {
                Id = Guid.NewGuid(),
                Title = title,
                Picture = "/images/playlists/default.jpg",
                Duration = duration,
                Rank = rank,
                IsDeleted = isDeleted,
                UserId = userId,
            };
        }
        public static Track CreateMockTrack(string title, int deezerId, Guid albumId, Guid artistId, Guid genreId, int duration, int rank = 100000)
        {
            return new Track
            {
                Id = Guid.NewGuid(),
                DeezerId = deezerId.ToString(),
                Title = title,
                SongURL = $"https://www.deezer.com/track/{deezerId.ToString()}",
                Duration = duration,
                Rank = rank,
                Preview = "https://cdns-preview-4.dzcdn.net/stream/c-4cc1f3a6e7655ebfe4621a75bcdaee34-4.mp3",
                AlbumId = albumId,
                ArtistId = artistId,
                GenreId = genreId,
            };
        }
        public static TrackPlaylist CreateMockTrackPlaylist(Guid playlistId, Guid trackId)
        {
            return new TrackPlaylist
            {
                TrackId = trackId,
                PlaylistId = playlistId,
            };
        }

        public static User CreateMockUser(string firstName, string lastName)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                IsDeleted = false,
                IsAdmin = false,
                IsBanned = false,
                UserName = $"{firstName.ToLower()}{lastName.ToLower()}@mail.com",
                Email = $"{firstName.ToLower()}{lastName.ToLower()}@mail.com",
                NormalizedUserName = $"{firstName.ToUpper()}{lastName.ToUpper()}@MAIL.COM",
                NormalizedEmail = $"{firstName.ToUpper()}{lastName.ToUpper()}@MAIL.COM",
            };
        }

    }
}
