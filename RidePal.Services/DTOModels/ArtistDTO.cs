using System;
using System.Collections.Generic;

namespace RidePal.Services.DTOModels
{
    public class ArtistDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }

        public ICollection<TrackDTO> Tracks { get; set; }// = new List<TrackDTO>();
        public ICollection<AlbumDTO> Albums { get; set; }// = new List<AlbumDTO>();
    }
}
