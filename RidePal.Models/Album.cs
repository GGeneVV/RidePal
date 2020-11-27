using Newtonsoft.Json;
using RidePal.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RidePal.Models
{
    public class Album : Entity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [JsonProperty("id")]
        public string DeezerId { get; set; }

        [Required]
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("cover")]
        public string Picture { get; set; }

        [Required]
        [JsonProperty("tracklist")]
        public string Tracklist { get; set; }

        [Required]
        [JsonProperty("artist")]
        public Artist Artist { get; set; }
        public Guid? ArtistId { get; set; }

        public ICollection<Track> Tracks { get; set; } = new List<Track>();
    }
}
