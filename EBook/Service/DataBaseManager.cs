using EBook.Context;
using EBook.Models;
using EBook.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBook.Service
{
    public class DataBaseManager : IBookService
    {
        private readonly IServiceProvider _serviceProvider;

        public DataBaseManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> AddBookAsync(BookDTO book)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<JwtContext>();

                    var newBook = new Book(book);

                    Console.WriteLine("GenreID received in AddBookAsync: " + book.GenreID);

                    var genreExists = await _context.Genres.AnyAsync(genre => genre.GenreID == book.GenreID);
                    if (!genreExists)
                    {
                        Console.WriteLine("Genre does not exist");
                        return "Genre does not exist";
                    }

                    _context.Books.Add(newBook);

                    foreach (var authorId in book.ID)
                    {
                        var author = await _context.Authors.FindAsync(authorId);
                        if (author != null)
                        {
                            var bookAuthor = new AuthorBookMappings
                            {
                                Book = newBook,
                                Author = author
                            };

                            _context.AuthorBookMappings.Add(bookAuthor);
                        }
                    }

                    await _context.SaveChangesAsync();
                    return "Book added successfully";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred in AddBookAsync: " + ex.Message);
                return "Book Add Unsuccessful";
            }
        }

        public async Task<string> UpdateBookAsync(Guid id, UpdateBook updateBook)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<JwtContext>();

                    var existingBook = await _context.Books.FirstOrDefaultAsync(book => book.BookID == id);

                    if (existingBook == null)
                    {
                        return "Book not found";
                    }

                    existingBook.Title = updateBook.Title;
                    existingBook.Description = updateBook.Description;
                    existingBook.ISBN = updateBook.ISBN;
                    existingBook.PublicationDate = updateBook.PublicationDate;
                    existingBook.Price = updateBook.Price;
                    existingBook.Language = updateBook.Language;
                    existingBook.Publisher = updateBook.Publisher;
                    existingBook.PageCount = updateBook.PageCount;
                    existingBook.AverageRating = updateBook.AverageRating;
                    existingBook.GenreID = updateBook.GenreID;
                    existingBook.Updated_At = DateTime.Now;

                    await _context.SaveChangesAsync();

                    return "Book update successful";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> DeleteBookAsync(Guid id)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<JwtContext>();

                    var book = await _context.Books.FindAsync(id);
                    if (book == null)
                        return "Book Not found";

                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();

                    return "Book Delete success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<JwtContext>();
                return await _context.Books.ToListAsync();
            }
        }

        public async Task<List<Book>> SearchBooksByTitleAsync(string title)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<JwtContext>();
                return await _context.Books.Where(book => book.Title.Contains(title)).ToListAsync();
            }
        }

        public async Task<List<Book>> GetBooksByGenreAsync(int genreId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<JwtContext>();
                return await _context.Books.Where(book => book.GenreID == genreId).ToListAsync();
            }
        }

        public async Task<List<Book>> GetBooksByAuthorAsync(Guid authorId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<JwtContext>();
                return await _context.AuthorBookMappings
                               .Where(authorbook => authorbook.AuthorID == authorId)
                               .Select(authorbook => authorbook.Book)
                               .ToListAsync();
            }
        }

        public List<Book> GetAllBook()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<JwtContext>();
                return _context.Books.ToList();
            }
        }
    }
}
