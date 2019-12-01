using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services.Auth
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

        private string GenerateToken(string userName, string email, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authConfiguration.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new [] 
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, string.Join(',', roles)),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        public async Task<Contract.User> AuthenticateAsync(string userName, string password)
        {
            var identityUser = _userManager.Users.SingleOrDefault(u => userName == u.UserName);
            if (identityUser == null)
                return null;
            
            var signInResult = await _signInManager.CheckPasswordSignInAsync(identityUser, password, false).ConfigureAwait(false);
            if (!signInResult.Succeeded)
                return null;

            return await GetUserContractAsync(identityUser).ConfigureAwait(false);
        }

        public async Task<Contract.User> RefreshTokenAsync(string userName)
        {
            var identityUser = _userManager.Users.SingleOrDefault(u => userName == u.UserName);
            return identityUser == null ? null : await GetUserContractAsync(identityUser).ConfigureAwait(false);
        }
        
        private async Task<Contract.User> GetUserContractAsync(IdentityUser identityUser)
        {
            var user = identityUser.ToContract();
            var roles = await _userManager.GetRolesAsync(identityUser).ConfigureAwait(false);
            return user.WithToken(GenerateToken(user.Username, user.Email, roles));
        }
    }
}