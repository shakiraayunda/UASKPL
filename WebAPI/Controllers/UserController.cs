using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IAuthManager _authManager;
        private readonly AppDbContext _context;


        public UserController(AppDbContext context, IAuthManager authManager)
        {
            _context = context;
            this._authManager = authManager;
        }


        // POST: api/User/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody] UserApiDto apiUserDto)
        {
            var errors = await _authManager.Register(apiUserDto);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok();
        }

        // POST: api/User/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            var authResponse = await _authManager.Login(loginDto);
            if (authResponse == null)
            {
                return Unauthorized();
            }
            return Ok(authResponse);
        }

        // GET: api/User
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserApi>>> GetUser()
        {
            return await _context.UserApi.ToListAsync();
        }


        // GET: api/User/5
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserApi>> GetUser(string id)
        {
            var user = await _context.UserApi.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("role/{email}")]
        public async Task<IList<string>> GetUserRoles(string email)
        {
            var roles = await _authManager.GetUserRoles(email);
            if (roles == null)
            {
                return null;
            }
            return roles;
        }


        // PUT: api/User/5
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] UserApiDto Users)
        {

            var errors = await _authManager.Update(id, Users);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok();
        }

        private bool UserExists(string email)
        {
            return _context.UserApi.Any(e => e.Email == email);
        }

        // DELETE: api/User/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.UserApi.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.UserApi.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
