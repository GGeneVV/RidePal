using System;
using System.Collections.Generic;

namespace RidePal.Services.DTOModels
{

    public class AlbumDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
        public string Tracklist { get; set; }
        public ArtistDTO Artist { get; set; }
        public Guid? ArtistId { get; set; }
        public IReadOnlyCollection<TrackDTO> Tracks { get; set; }// = new List<TrackDTO>();
    }
}
