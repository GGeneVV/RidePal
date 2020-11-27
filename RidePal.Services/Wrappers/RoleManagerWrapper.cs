using Microsoft.AspNetCore.Identity;
using RidePal.Services.Exceptions;
using RidePal.Services.Wrappers.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Services.Wrappers
{
    public class RoleManagerWrapper : IRoleManagerWrapper
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleManagerWrapper(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<IdentityResult> CreateRoleAsync(string name)
        {
            var role = await this.roleManager.FindByNameAsync(name);

            if (role != null)
            {
                throw new EntityInvalidException("Role already exists!");
            }

            return await this.roleManager.CreateAsync(new IdentityRole(name));
        }

        public async Task<IdentityResult> DeleteRoleAsync(string name)
        {
            var role = await this.FindByNameAsync(name);

            if (role == null)
            {
                throw new EntityInvalidException("Role not found!");
            }

            return await this.roleManager.DeleteAsync(role);
        }

        public async Task<IdentityRole> FindByNameAsync(string name)
        {
            var role = await this.roleManager.FindByNameAsync(name);

            if (role == null)
            {
                throw new EntityInvalidException("Role not found!");
            }

            return role;
        }

        public IQueryable<IdentityRole> GetAllRoles()
        {
            var roles = this.roleManager.Roles;

            return roles;
        }

        public async Task<IdentityResult> UpdateRoleAsync(IdentityRole roleModel)
        {
            if (roleModel.NormalizedName == "ADMIN")
            {
                throw new EntityInvalidException("You are not allowed to edit the Admin role!");
            }

            return await this.roleManager.UpdateAsync(roleModel);
        }
    }
}
