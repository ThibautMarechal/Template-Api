using Contract;
using Microsoft.AspNetCore.Identity;

namespace Api.Mappers
{
    public static class UserExtenions
    {
        public static User ToContract(this IdentityUser identityUser)
        {
            if (identityUser == null)
                return null;
            return new User
            {
                Email = identityUser.Email,
                Username = identityUser.UserName,
            };
        }

        public static UserWithToken WithToken(this User user, string token)
        {
            if (user == null)
                return null;
            return new UserWithToken
            {
                Email = user.Email,
                Username = user.Username,
                Token = token
            };
        }
    }
}