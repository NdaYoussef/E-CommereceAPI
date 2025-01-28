using Microsoft.AspNetCore.Identity;

namespace TestToken.Data
{
    public static class SeedData
    {
        public static async Task seedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Customer" };
            //bool IsRoleSeeded = await roleManager.RoleExistsAsync("Admin");
            foreach (string role in roles)
            {
                if(! await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
