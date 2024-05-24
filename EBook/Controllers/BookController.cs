using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using EBook.Models;
using EBook.Services.Iface;
using EBook.Services.Interf;
using EBook.Services.Interface;
using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using EBook.Context;


namespace EBook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]


    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly ILoginService _loginService;
        private readonly JwtContext _context;

        public BookController(IBookService bookService, IAuthorService authorService, ILoginService loginService, JwtContext context)
        {
            _bookService = bookService;
            _authorService = authorService;
            _loginService = loginService;
            _context = context;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("GetAllBooks")]
        public async Task<ActionResult> GetAllBooks()
        {
            return Ok(await _bookService.GetAllBooksAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddBook")]
        public async Task<ActionResult> AddBook([FromBody] BookDTO book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _bookService.AddBookAsync(book);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("DeleteBook/{id}")]
        public async Task<ActionResult> DeleteBook(Guid id)
        {
            try
            {
                var result = await _bookService.DeleteBookAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [Route("UpdateBook/{id}")]
        public async Task<ActionResult> UpdateBook(Guid id, [FromBody] UpdateBook updateBookDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _bookService.UpdateBookAsync(id, updateBookDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("GetAllAuthors")]
        public async Task<ActionResult> GetAllAuthors()
        {
            return Ok(await _authorService.GetAllAuthorsAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddAuthor")]
        public async Task<ActionResult> AddAuthor([FromBody] AuthorDTO author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authorService.AddAuthorAsync(author);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("DeleteAuthor/{id}")]
        public async Task<ActionResult> DeleteAuthor(Guid id)
        {
            try
            {
                var result = await _authorService.DeleteAuthorAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [Route("UpdateAuthor/{id}")]
        public async Task<ActionResult> UpdateAuthor(Guid id, [FromBody] UpdateAuthor updateauthor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authorService.UpdateAuthorAsync(id, updateauthor);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("SearchBooksByTitle")]
        public async Task<ActionResult> SearchBooksByTitle(string title)
        {
            return Ok(await _bookService.SearchBooksByTitleAsync(title));
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("GetBooksByGenre/{genreID}")]
        public async Task<ActionResult> GetBooksByGenre(int genreID)
        {
            return Ok(await _bookService.GetBooksByGenreAsync(genreID));
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("GetBooksByAuthor/{authorID}")]
        public async Task<ActionResult> GetBooksByAuthor(Guid authorID)
        {
            return Ok(await _bookService.GetBooksByAuthorAsync(authorID));
        }


        [HttpPost]
        [Route("Signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Signup([FromBody] UserDTO userSignup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userSignup.Username);
            if (existingUser != null)
            {
                return Conflict("Username already exists");
            }

            try
            {
                string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userSignup.Password, 12);

                var newUser = new Users
                {

                    Username = userSignup.Username,
                    Password = passwordHash,
                    Role = Roles.User.ToString()
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPost]
        [Route("Validate")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateToken([FromBody] UserDTO login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var role = await _loginService.RoleAsync(login);
                if (string.IsNullOrEmpty(role))
                {
                    return Unauthorized("Invalid username or password");
                }

                var jwtToken = await _loginService.GenerateToken(login, role);

                return Ok(jwtToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("An error occurred during login");
            }
        }

    }
}
