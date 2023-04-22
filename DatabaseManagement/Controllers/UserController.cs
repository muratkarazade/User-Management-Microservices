using DatabaseManagement.Data;
using DatabaseManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatabaseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.Include(u => u.AuthenticationCredential).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }







        [HttpGet("{id}")]
        public async Task<ActionResult<UserWithCredentials>> GetUser(int id)
        {
            var user = await _context.Users.Include(u => u.AuthenticationCredential).FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            
            var userWithCredentials = new UserWithCredentials(user)
            {
                Username = user.AuthenticationCredential.Username,
                Password = user.AuthenticationCredential.Password,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Id = user.Id
                
            };

            return userWithCredentials;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserWithCredentials userWithCredentials)
        {
            var user = userWithCredentials.ToUser();
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var authCredential = userWithCredentials.ToAuthenticationCredential();
            _context.Entry(authCredential).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // POST: api/User/CreateUser
        [HttpPost("CreateUser")]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserWithCredentials userWithCredentials)
        {
            // Yeni User nesnesi oluşturma
            User user = new User
            {
                Email = userWithCredentials.Email,
                Name = userWithCredentials.Name,
                Surname = userWithCredentials.Surname
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // AuthenticationCredential nesnesi oluşturma
            AuthenticationCredential authenticationCredential = new AuthenticationCredential
            {
                UserId = user.Id,
                Username = userWithCredentials.Username,
                Password = userWithCredentials.Password
            };

            _context.AuthenticationCredentials.Add(authenticationCredential);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var authCredential = await _context.AuthenticationCredentials.FirstOrDefaultAsync(ac => ac.UserId == id);
            if (authCredential != null)
            {
                _context.AuthenticationCredentials.Remove(authCredential);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        [HttpGet("Authenticate")]
        public async Task<ActionResult<User>> AuthenticateUser(string username, string password)
        {
            var authenticationCredential = await _context.AuthenticationCredentials
                .Include(ac => ac.User)
                .FirstOrDefaultAsync(ac => ac.Username == username && ac.Password == password);

            if (authenticationCredential == null)
            {
                return Unauthorized();
            }

            return authenticationCredential.User;
        }

    }
}