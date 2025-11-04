using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging; 
using MyPortfolio.Core.Entities;
using System.Threading.Tasks;

namespace MyPortfolio.Data
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedRolesAndSuperAdminAsync(IServiceProvider services, ILogger logger)
        {
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "SuperAdmin", "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (result.Succeeded)
                    {
                        logger.LogInformation($"Role '{roleName}' created successfully.");
                    }
                }
            }

            var superAdminEmail = "superadmin@gmail.com"; 
            var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);

            if (superAdminUser == null)
            {
                var user = new AppUser
                {
                    UserName = "superadmin", 
                    Email = superAdminEmail,
                    EmailConfirmed = true
                };


                string userPassword = "SuperAdmin123!"; 

                var createResult = await userManager.CreateAsync(user, userPassword);

                if (createResult.Succeeded)
                {
                    logger.LogInformation($"User '{user.UserName}' created successfully.");

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