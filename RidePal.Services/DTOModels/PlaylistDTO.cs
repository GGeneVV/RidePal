using System;
using System.Collections.Generic;

namespace RidePal.Services.DTOModels
{
    public class PlaylistDTO
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
        public UserDTO User { get; set; }

        public ICollection<TrackPlaylistDTO> TrackPlaylists { get; set; }// = new List<TrackPlaylistDTO>();
    }
}
