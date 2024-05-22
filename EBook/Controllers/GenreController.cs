using EBook.Context;
using EBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly JwtContext _context;

        public GenreController(JwtContext context)
        {
            _context = context;
        }

        [HttpPost("AddGenre")]
        public async Task<ActionResult> AddGenre([FromBody] GenereDTO genreDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var genre = new Genre
            {
               // GenreID = genreDTO.GenreID,
                Name = genreDTO.Name,
                
            };

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return Ok("Genre added successfully");
        }
    }
}

