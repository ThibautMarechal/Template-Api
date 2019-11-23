using System.Security.Claims;
using System.Threading.Tasks;
using Api.Services;
using Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/auth")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UsersController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody]User userParam)
        {
            var user = await _authService.Authenticate(userParam.Username, userParam.Password).ConfigureAwait(false);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }
        
        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            if (userId == null)
                return BadRequest(new { message = "Bad token" });

            var user = await _authService.RefreshToken(userId).ConfigureAwait(false);
            if (user == null)
                return BadRequest(new { message = "Bad token" });
            
            return Ok(user);
        }
    }
}