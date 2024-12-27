using deneme135.Models;
using Microsoft.AspNetCore.Identity;

public static class DataSeeder
{
    public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var roleExist = await roleManager.RoleExistsAsync("Admin");
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        var user = await userManager.FindByEmailAsync("admin@admin.com");
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com"
            };

            var result = await userManager.CreateAsync(user, "AdminPassword123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
