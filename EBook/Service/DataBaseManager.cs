using EBook.Models;
using EBook.Services.Interface;
using EBook.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBook.Service
{
    public class DataBaseManager : IBookService
    {
        private readonly JwtContext _context;

        public DataBaseManager(JwtContext context)
        {
            _context = context;
        }

        public List<Book> GetAllBook()
        {
            return _context.Books.ToList();
        }

        public async Task<string> AddBookAsync(BookDTO book)
        {
            try
            {
                var newBook = new Book(book);

               
                Console.WriteLine("GenreID received in AddBookAsync: " + book.GenreID);

               
                var genreExists = await _context.Genres.AnyAsync(g => g.GenreID == book.GenreID);
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
                var existingBook = await _context.Books.FirstOrDefaultAsync(b => b.BookID == id);

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
               // existingBook.TotalRating = updateBook.TotalRating;
                existingBook.GenreID = updateBook.GenreID;
                existingBook.Updated_At = DateTime.Now;

                await _context.SaveChangesAsync();

                return "Book update successful";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    

    public async Task<string> DeleteBookAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return "Book Not found";

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return "Book Delete success";
        }

        public async Task<List<Book>> GetAllBooksAsync() 
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<List<Book>> SearchBooksByTitleAsync(string title)
        {
            return await _context.Books.Where(b => b.Title.Contains(title)).ToListAsync();
        }

        public async Task<List<Book>> GetBooksByGenreAsync(int genreId)
        {
            return await _context.Books.Where(b => b.GenreID == genreId).ToListAsync();
        }

        public async Task<List<Book>> GetBooksByAuthorAsync(Guid authorId)
        {
            return await _context.AuthorBookMappings
                           .Where(ab => ab.AuthorID == authorId)
                           .Select(ab => ab.Book)
                           .ToListAsync();
        }
    }
}
