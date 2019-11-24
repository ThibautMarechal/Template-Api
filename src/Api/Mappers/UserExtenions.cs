using Contract;
using Microsoft.AspNetCore.Identity;

namespace Api.Mappers
{
    public static class UserExtenions
    {
        public static User ToContract(this IdentityUser identityUser)
        {
            if (identityUser == null)
            {
                return null;
            }
            return new User
            {
                Email = identityUser.Email,
                Username = identityUser.UserName,
            };
        } 
    }
}