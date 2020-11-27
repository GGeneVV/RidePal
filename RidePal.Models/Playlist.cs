using RidePal.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RidePal.Models
{
    public class Playlist : Entity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
        public int Duration { get; set; }
        public int Rank { get; set; }
        public bool Enabled { get; set; }
        public bool IsArtistRepeated { get; set; }
        public bool IsTopTracksEnabled { get; set; }

        public Guid? UserId { get; set; }
        public User User { get; set; }

        public ICollection<TrackPlaylist> TrackPlaylists { get; set; } = new List<TrackPlaylist>();
    }
}
