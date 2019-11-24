using System;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Mappers;
using Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthConfiguration _authConfiguration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthService(AuthConfiguration authConfiguration, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _authConfiguration = authConfiguration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private string GenerateToken(string userName, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authConfiguration.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new [] 
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Email, email),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        public async Task<User> Authenticate(string userName, string password)
        {
            var identityUser = _userManager.Users.SingleOrDefault(u => userName == u.UserName);
            if (identityUser == null)
                return null;
            
            var signInResult = await _signInManager.CheckPasswordSignInAsync(identityUser, password, false).ConfigureAwait(false);
            if (!signInResult.Succeeded)
                return null;

            return GetUserContract(identityUser);
        }

        public Task<User> RefreshToken(string userName)
        {
            var identityUser = _userManager.Users.SingleOrDefault(u => userName == u.UserName);
            return Task.FromResult(identityUser == null ? null : GetUserContract(identityUser));
        }
        
        private User GetUserContract(IdentityUser identityUser)
        {
            var user = identityUser.ToContract();
            user.Token = GenerateToken(user.Username, user.Email);
            return user;
        }
    }
}