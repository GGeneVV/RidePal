using Newtonsoft.Json;
using RidePal.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RidePal.Models
{
    public class Track : Entity
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        [JsonProperty("id")]
        public string DeezerId { get; set; }

        [Required]
        [JsonProperty("title")]
        public string Title { get; set; }

        [Required]
        [JsonProperty("link")]
        public string SongURL { get; set; }

        [Required]
        [JsonProperty("duration")]
        public int Duration { get; set; }

        [Required]
        [JsonProperty("rank")]
        public int Rank { get; set; }

        [Required]
        [JsonProperty("release_date")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [JsonProperty("preview")]
        public string Preview { get; set; }

        [Required]
        [JsonProperty("album")]
        public Album Album { get; set; }

        public Guid? AlbumId { get; set; }

        [Required]
        [JsonProperty("artist")]
        public Artist Artist { get; set; }
        public Guid? ArtistId { get; set; }

        public Genre Genre { get; set; }
        public Guid? GenreId { get; set; }

        public ICollection<TrackPlaylist> TrackPlaylists { get; set; } = new List<TrackPlaylist>();

    }
}
