using System;
using System.Threading.Tasks;
using Api.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public static class UserCreator
    {
        public static async Task Init(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            
            if (!await userManager.Users.AnyAsync().ConfigureAwait(false))
            {
                var adminConfig = serviceProvider.GetService<AdminConfiguration>();
                var adminUser = new IdentityUser
                {
                    Email = adminConfig.Email,
                    UserName = adminConfig.UserName
                };
                adminUser.PasswordHash = userManager.PasswordHasher.HashPassword(adminUser, adminConfig.Password);
                await userManager.CreateAsync(adminUser).ConfigureAwait(false);
            }
        }
    }
}