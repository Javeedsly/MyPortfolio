using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging; // <-- Bu sətir olduğundan əmin olun
using MyPortfolio.Core.Entities;
using System.Threading.Tasks;

namespace MyPortfolio.Data
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedRolesAndSuperAdminAsync(IServiceProvider services, ILogger logger)
        {
            // Lazımi servisləri əldə edirik
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "SuperAdmin", "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Rol mövcud deyilsə, yarat
                    var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (result.Succeeded)
                    {
                        logger.LogInformation($"Role '{roleName}' created successfully.");
                    }
                }
            }

            // SuperAdmin istifadəçisini yarat
            var superAdminEmail = "superadmin@example.com"; // <-- DƏYİŞİN
            var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);

            if (superAdminUser == null)
            {
                var user = new AppUser
                {
                    UserName = "superadmin", // <-- DƏYİŞİN
                    Email = superAdminEmail,
                    EmailConfirmed = true
                };


                string userPassword = "YourSuperStrongP@ssword1!"; // <-- DƏYİŞİN

                var createResult = await userManager.CreateAsync(user, userPassword);

                if (createResult.Succeeded)
                {
                    logger.LogInformation($"User '{user.UserName}' created successfully.");

                    // İstifadəçiyə "SuperAdmin" rolunu ver
                    await userManager.AddToRoleAsync(user, "SuperAdmin");
                    logger.LogInformation($"User '{user.UserName}' added to 'SuperAdmin' role.");
                }
                else
                {
                    foreach (var error in createResult.Errors)
                    {
                        logger.LogError($"Error creating user '{user.UserName}': {error.Description}");
                    }
                }
            }
        }
    }
}