using System;

namespace RidePal.Models.Abstracts
{
    public abstract class Entity
    {
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
