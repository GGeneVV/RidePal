using System.Collections.Generic;

namespace RidePal.Services.DTOModels.Configurations
{
    public class PlaylistConfigDTO
    {
        public string Title { get; set; }
        public bool UseTopTracks { get; set; }
        public bool AllowTracksFromSameArtist { get; set; }
        public bool IsAdvanced { get; set; }
        public IList<PlaylistGenreConfigDTO> GenreConfigs { get; set; } = new List<PlaylistGenreConfigDTO>();
    }
}
