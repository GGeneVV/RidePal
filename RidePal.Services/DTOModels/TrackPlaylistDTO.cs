using System;

namespace RidePal.Services.DTOModels
{
    //TODO: we don't need this
    public class TrackPlaylistDTO
    {
        public Guid TrackId { get; set; }
        public TrackDTO Track { get; set; }

        public Guid PlaylistId { get; set; }
        public PlaylistDTO Playlist { get; set; }
    }
}
