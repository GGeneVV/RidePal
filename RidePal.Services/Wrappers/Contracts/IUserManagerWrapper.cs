using Microsoft.AspNetCore.Identity;
using RidePal.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RidePal.Services.Wrappers.Contracts
{
    public interface IUserManagerWrapper
    {
        Task<User> FindByNameAsync(string username);

        Guid FindIdByNameAsync(string username);
        Task<User> FindByIdAsync(string id);

        Task<IdentityResult> AddToRoleAsync(User user, string role);

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IList<string>> GetAllRoles(string userName);

        Task<string> GetRole(string userName);

        Task<User> GetUserAsync(ClaimsPrincipal claimsPrincipal);
    }
}
