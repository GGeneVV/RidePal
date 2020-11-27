using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace RidePal.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string Image { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBanned { get; set; }

        public bool IsAdmin { get; set; }

        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();


    }
}
