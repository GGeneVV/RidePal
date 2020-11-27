using System;

namespace RidePal.Web.Models.EditVM
{
    public class EditPlaylistVM
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool Revive { get; set; }
    }
}
