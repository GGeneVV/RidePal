using System.Collections.Generic;

namespace RidePal.Web.Models
{
    public class PlaylistConfigVM
    {
        public string Title { get; set; }
        public bool UseTopTracks { get; set; }
        public bool AllowTracksFromSameArtist { get; set; }
        public bool IsAdvanced { get; set; }

        //[BindProperty]
        public IList<GenreConfigVM> GenreConfigs { get; set; }// = new List<GenreConfigVM>();
    }
}
