using Microsoft.AspNetCore.Identity;


namespace ProductWeb.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;


        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public async Task<bool> Add(RoleDto roleDto)
        {
            var identityRole = new IdentityRole
            {
                Name = roleDto.Name,
                NormalizedName = _roleManager.NormalizeKey(roleDto.Name)
            };


            var result = await _roleManager.CreateAsync(identityRole);


            if (!result.Succeeded) return false;

            return true;


        }


        public async Task<bool> Delete(string name)
        {
            var identityRole = await Find(name);


            if (identityRole == null) return false;


            //ตรวจสอบมีผู้ใช้บทบาทนี้หรือไม่
            var usersInRole = await _userManager.GetUsersInRoleAsync(name);
            if (usersInRole.Count != 0) return false;


            var result = await _roleManager.DeleteAsync(identityRole);


            if (!result.Succeeded) return false;


            return true;
        }


        public async Task<IdentityRole> Find(string name)
        {
            var identityRole = await _roleManager.FindByNameAsync(name);
            return identityRole;
        }


        public async Task<List<IdentityRole>> GetAll()
        {
            var result = await _roleManager.Roles.ToListAsync();
            return result;
        }


        public async Task<bool> Update(RoleUpdateDto roleUpdateDto)
        {
            var identityRole = await Find(roleUpdateDto.Name);


            if (identityRole == null) return false;


            identityRole.Name = roleUpdateDto.UpdateName;
            identityRole.NormalizedName = _roleManager.NormalizeKey(roleUpdateDto.UpdateName);


            var result = await _roleManager.UpdateAsync(identityRole);


            if (!result.Succeeded) return false;


            return true;
        }


    }

}
