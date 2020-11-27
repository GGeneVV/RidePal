using System.Collections.Generic;
using System.Linq;

namespace RidePal.Web.Models
{
    public class HomeIndexVM
    {
        //For Generating playlist
        public PlaylistConfigVM PlaylistConfig { get; set; }


        public IEnumerable<TrackVM> PopularTracks { get; set; }
        public IEnumerable<TrackVM> TopTracks { get; set; }
        public IEnumerable<ArtistVM> TopArtists { get; set; }
        public IEnumerable<AlbumVM> TopAlbums { get; set; }
        public IEnumerable<PlaylistVM> TopPlaylists { get; set; }
    }
}
