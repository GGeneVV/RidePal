using System;

namespace RidePal.Services.Exceptions
{
    public class EntityInvalidException : Exception
    {
        public EntityInvalidException(string message)
            : base(message)
        {
        }
    }
}