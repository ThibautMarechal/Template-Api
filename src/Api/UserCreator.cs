using System;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Constants;
using Api.Exceptions.UserCreatorExceptions;
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
                var userCreationResult = await userManager.CreateAsync(adminUser).ConfigureAwait(false);
                if (!userCreationResult.Succeeded)
                    throw new CreateUserException();
                
                if (!await roleManager.Roles.AnyAsync().ConfigureAwait(false))
                {
                    var role = new IdentityRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = Role.Admin,
                    };
                    var roleCreationResult = await roleManager.CreateAsync(role).ConfigureAwait(false);
                    if (!roleCreationResult.Succeeded)
                        throw new CreateRoleException();

                    var applyRoleResult = await userManager.AddToRoleAsync(adminUser, Role.Admin).ConfigureAwait(false);
                    if(!applyRoleResult.Succeeded)
                        throw new ApplyRoleException();
                }
            }
        }
    }
}