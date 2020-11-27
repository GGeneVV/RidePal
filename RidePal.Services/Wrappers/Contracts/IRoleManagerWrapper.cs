using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Services.Wrappers.Contracts
{
    public interface IRoleManagerWrapper
    {
        public interface IRoleManagerWrapper
        {
            IQueryable<IdentityRole> GetAllRoles();

            Task<IdentityResult> CreateRoleAsync(string name);

            Task<IdentityRole> FindByNameAsync(string name);

            Task<IdentityResult> DeleteRoleAsync(string name);

            Task<IdentityResult> UpdateRoleAsync(IdentityRole role);
        }
    }
}
