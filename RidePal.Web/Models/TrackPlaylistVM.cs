using System;

namespace RidePal.Web.Models
{
    public class TrackPlaylistVM
    {
        public Guid TrackId { get; set; }
        public TrackVM Track { get; set; }

        public Guid PlaylistId { get; set; }
        public PlaylistVM Playlist { get; set; }
    }
}
