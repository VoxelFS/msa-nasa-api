using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using msa_nasa_project.Data;
using msa_nasa_project.Models;

namespace msa_nasa_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ImageDbContext _context;

        public ImagesController(ImageDbContext context) => _context = context;


        [HttpGet("{userID}")]
        [ProducesResponseType(typeof(Images), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserID(string userID)
        {
            var images = await _context.Images.FirstOrDefaultAsync(image => image.UserID == userID);
            if (images == null)
            {
                Images[] array = Array.Empty<Images>();
                return NotFound(array);
            }
            var groupedData = await _context.Images.Where(image => image.UserID == userID).ToListAsync();
            return Ok(groupedData);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Images images)
        {
            var imageToCheck = await _context.Images.FirstOrDefaultAsync(image1 => (image1.UserID == images.UserID && image1.Image == images.Image));
            if (imageToCheck == null)
            {
                await _context.Images.AddAsync(images);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetByUserID), new { userID = images.UserID }, images);
            }
            else
            {
                return BadRequest("Already exists");
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Images image)
        {
            var images = await _context.Images.FirstOrDefaultAsync(image1 => (image1.UserID == image.UserID && image1.Image == image.Image));
            if(images == null) return NotFound();

            _context.Images.Remove(images);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
