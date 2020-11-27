using System;
using System.Collections.Generic;

namespace RidePal.Web.Models
{
    public class PlaylistVM
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
        public int Duration { get; set; }
        public int Rank { get; set; }
        public bool Enabled { get; set; }
        public bool IsArtistRepeated { get; set; }
        public bool IsTopTracksEnabled { get; set; }

        public Guid? UserId { get; set; }
        public UserVM User { get; set; }

        public ICollection<TrackPlaylistVM> TrackPlaylists { get; set; }
    }
}
