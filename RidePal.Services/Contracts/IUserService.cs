using RidePal.Services.DTOModels;
using System;
using System.Linq;

namespace RidePal.Services.Contracts
{
    public interface IUserService
    {
        public string Authenticate(string username, string password);
        public bool ValidateUserById(Guid userId);
        public IQueryable<UserDTO> GetAllUsersAsync(int? pageNumber = 1,
            string sortOrder = "",
            string currentFilter = "",
            string searchString = "");
    }
}
