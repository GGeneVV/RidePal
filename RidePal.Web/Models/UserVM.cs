using System.Collections.Generic;

namespace RidePal.Web.Models
{
    public class UserVM
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public ICollection<PlaylistVM> Playlists { get; set; } = new List<PlaylistVM>();
    }
}
