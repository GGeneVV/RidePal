using System.Collections.Generic;

namespace RidePal.Web.Models
{
    public class BrowseAllVM
    {
        public IReadOnlyCollection<TrackVM> Tracks { get; set; }
        public IReadOnlyCollection<ArtistVM> Artists { get; set; }
        public IReadOnlyCollection<AlbumVM> Albums { get; set; }
        public IReadOnlyCollection<PlaylistVM> Playlists { get; set; }
    }
}
