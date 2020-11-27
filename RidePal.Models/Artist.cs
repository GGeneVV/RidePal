using Newtonsoft.Json;
using RidePal.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RidePal.Models
{
    public class Artist : Entity
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        [JsonProperty("id")]
        public string DeezerId { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [Required]
        [JsonProperty("tracklist")]
        public string TrackListURL { get; set; }

        public ICollection<Track> Tracks { get; set; } = new List<Track>();

        public ICollection<Album> Albums { get; set; } = new List<Album>();
    }
}
