using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Constants;
using Api.Exceptions.UserServiceExceptions;
using Api.Services;
using Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("/user")]
    [Authorize(Roles = Auth.AdminRole)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userService.GetAllAsync().ConfigureAwait(false);
        }        
        
        [HttpGet("{userName}")]
        public async Task<IActionResult> GetUserAsync([FromRoute(Name = "userName")] string  userName)
        {
            try
            {
                var user = await _userService.GetByUserNameAsync(userName).ConfigureAwait(false);
                return Ok(user);
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
        }       
        
        [HttpPost("{userName}")]
        public async Task<IActionResult> CreateUserAsync([FromRoute(Name = "userName")] string  userName, [FromBody] UserIn userIn)
        {
            try
            {
                var user = await _userService.CreateUserAsync(new User
                {
                    Email = userIn.Email,
                    Username = userName
                }, userIn.Password).ConfigureAwait(false);
                return Created($"{Request.Path}/{userName}", user);
            }
            catch (UserCreateException e)
            {
                return BadRequest(e.Errors);
            }
            catch (UserAlreadyExistException)
            {
                return Conflict();
            }
        }    
        
        [HttpPut("{userName}")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute(Name = "userName")] string  userName, [FromBody] UserIn userIn)
        {
            try
            {
                var user =  await _userService.UpdateUserAsync(new User
                {
                    Email = userIn.Email,
                    Username = userName
                }, userIn.Password).ConfigureAwait(false);
                return Ok(user);
            }
            catch (UserUpdateException e)
            {
                return BadRequest(e.Errors);
            }
            catch (UserAdminException)
            {
                return Unauthorized("User admin cannot be updated.");
            }
        }

        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute(Name = "userName")] string  userName)
        {
            try
            {
                await _userService.DeleteByUserNameAsync(userName).ConfigureAwait(false);
                return NoContent();
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (UserAdminException)
            {
                return Unauthorized("User admin cannot be deleted.");
            }
        }
    }
}