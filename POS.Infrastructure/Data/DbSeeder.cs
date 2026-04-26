using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using POS.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Infrastructure.Data
{
    public class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider service)
        {
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();

            //Seed Roles here

            string[] roles = { "Admin", "User" };
            
            foreach (var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            //Seed Admin User here

            var adminEmail = "admin@pos.com";
            var adminPass = "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if(adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                };

                await userManager.CreateAsync(adminUser, adminPass);
                await userManager.AddToRoleAsync(adminUser, "Admin");
            } 
        }
    }
}
