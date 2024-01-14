using Microsoft.AspNetCore.Identity;
using ProductWeb.Models;

namespace ProductWeb.Services
{
    public interface IRoleService
    {
        Task<List<IdentityRole>> GetAll();
        Task<bool> Add(RoleDto roleDto);
        Task<bool> Update(RoleUpdateDto roleUpdateDto);
        Task<bool> Delete(string name);
        Task<IdentityRole> Find(string name);
    }

}
