using Newtonsoft.Json;
using System.Collections.Generic;

namespace RidePal.Models.DataSource
{
    public class DataGenre
    {
        [JsonProperty("data")]
        public List<Genre> Genres { get; set; }
    }
}
