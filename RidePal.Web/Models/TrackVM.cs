using System;

namespace RidePal.Web.Models
{
    public class TrackVM
    {
        public Guid Id { get; set; }
        public string DeezerId { get; set; }
        public string Title { get; set; }
        public string SongURL { get; set; }
        public int Duration { get; set; }
        public int Rank { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Preview { get; set; }
        public AlbumVM Album { get; set; }
        public Guid? AlbumId { get; set; }
        public ArtistVM Artist { get; set; }
        public Guid? ArtistId { get; set; }
        public GenreVM Genre { get; set; }
        public Guid? GenreId { get; set; }
    }
}
