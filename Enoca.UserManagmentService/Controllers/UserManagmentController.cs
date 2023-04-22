using Enoca.UserManagmentService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Enoca.UserManagmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagmentController : ControllerBase
    {
        private readonly UserApiClient _userApiClient;

        public UserManagmentController(UserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userApiClient.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userApiClient.GetUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            var createdUser = await _userApiClient.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            await _userApiClient.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userApiClient.GetUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userApiClient.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
