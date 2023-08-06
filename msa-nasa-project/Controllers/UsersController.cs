using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using msa_nasa_project.Data;
using msa_nasa_project.Models;

namespace msa_nasa_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersDbContext _context;

        public UsersController(UsersDbContext context) => _context = context;

        [HttpGet]
        public async Task<IEnumerable<Users>> Get()
            => await _context.Users.ToListAsync();

        [HttpGet("{email}")]
        [ProducesResponseType(typeof(Users), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user=>user.Email==email);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Users user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByEmail), new { email = user.Email }, user);
        }

        [HttpDelete("{email}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(string email)
        {
            var userToDelete = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
            if (userToDelete == null) return NotFound();

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
