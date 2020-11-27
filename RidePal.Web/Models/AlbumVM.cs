using System;
using System.Collections.Generic;

namespace RidePal.Web.Models
{
    public class AlbumVM
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
        public string Tracklist { get; set; }
        public ArtistVM Artist { get; set; }
        public Guid? ArtistId { get; set; }
        public IReadOnlyCollection<TrackVM> Tracks { get; set; }// = new List<TrackVM>();
    }
}
