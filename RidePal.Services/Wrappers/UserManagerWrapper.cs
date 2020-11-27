using Microsoft.AspNetCore.Identity;
using RidePal.Models;
using RidePal.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace RidePal.Services.Wrappers
{
    public class UserManagerWrapper : IUserManagerWrapper
    {
        private readonly UserManager<User> _userManager;

        public UserManagerWrapper(UserManager<User> userManager)
        {
            this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }


        public async Task<User> FindByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<User> FindByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IdentityResult> AddToRoleAsync(User user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IList<string>> GetAllRoles(string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<string> GetRole(string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            return role;
        }

        public Guid FindIdByNameAsync(string username)
        {
            var id = _userManager.FindByNameAsync(username);
            return id.Result.Id;

        }

        public async Task<User> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);

            return user;
        }
    }
}
