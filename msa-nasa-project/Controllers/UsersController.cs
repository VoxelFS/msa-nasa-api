using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using msa_nasa_project.Data;
using msa_nasa_project.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace msa_nasa_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(UsersDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("{name}")]
        [ProducesResponseType(typeof(Users), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByName(string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user=>user.Name==name);
            return user == null ? NotFound("User Not Found") : Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Users user)
        {
            var userToCheck = await _context.Users.FirstOrDefaultAsync(user1 => user1.Name == user.Name);
            if (userToCheck != null)
            {
                return BadRequest(false);
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByName), new { name = user.Name }, user);
        }

        [HttpDelete("{name}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(string name)
        {
            var userToDelete = await _context.Users.FirstOrDefaultAsync(user => user.Name == name);
            if (userToDelete == null) return NotFound();

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Users user)
        {
            var userToCheck = await _context.Users.FirstOrDefaultAsync(user1 => user1.Name == user.Name);
            if (userToCheck == null)
            {
                return BadRequest("Email is invalid or incorrect password.");
            }

            if (user.Email == userToCheck.Email && userToCheck.Name.ToLower() == user.Name.ToLower())
            {
                var token = CreateAuthToken(user);
                return Ok(true);
                
            }

            return BadRequest("Email is invalid or incorrect password.");
        }

        private string CreateAuthToken(Users user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes("dAUaurpRcvKDGhyxikkzyrNv5jhfbA3awRgAfbv1Tf2uNad12dsavtrtwS");

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Name),
                    new Claim(JwtRegisteredClaimNames.Email, user.Name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }),

                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha384Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
