using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Configuration;
using Contract;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthConfiguration _authConfiguration;
        private readonly IUserService _userService;

        public AuthService(AuthConfiguration authConfiguration, IUserService userService)
        {
            _authConfiguration = authConfiguration;
            _userService = userService;
        }

        private string GenerateToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authConfiguration.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new [] 
                {
                    new Claim(ClaimTypes.Name, userId),
                    
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _userService.GetUserByUserNameAndPassword(username, password).ConfigureAwait(false);
            if (user == null)
                return null;
            
            user.Token = GenerateToken(user.Id);
            user.Password = null;
            return user;
        }

        public async Task<User> RefreshToken(string userId)
        {
            var user = await _userService.GetUserById(userId).ConfigureAwait(false);
            if (user == null)
                return null;

            user.Token = GenerateToken(userId);
            user.Password = null;
            return user;
        }
    }
}