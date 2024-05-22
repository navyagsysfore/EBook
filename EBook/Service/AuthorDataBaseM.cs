using EBook.Models;
using EBook.Services.Interf;
using EBook.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBook.Service
{
    public class AuthorDataBaseM : IAuthorService
    {
        private readonly JwtContext _context;

        public AuthorDataBaseM(JwtContext context)
        {
            _context = context;
        }

        public List<Author> GetAllAuthors()
        {
            return _context.Authors.ToList();
        }

        public async Task<Author> GetAuthorByIdAsync(Guid ID) 
        {
            return await _context.Authors.FirstOrDefaultAsync(author => author.ID == ID);
        }

        public async Task<string> AddAuthorAsync(AuthorDTO authors)
        {
            try
            {
                Author author = new Author(authors);
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return "Author Add Unsuccessful";
            }

            return "Author added successfully";
        }

        public async Task<string> UpdateAuthorAsync(Guid id, UpdateAuthor updateauthor)
        {
            try
            {
                var existingAuthor = await _context.Authors.FirstOrDefaultAsync(author => author.ID == id);

                if (existingAuthor == null)
                {
                    return "Author not found";
                }
                existingAuthor.FirstName = updateauthor.FirstName;
                existingAuthor.LastName = updateauthor.LastName;
                existingAuthor.Biography = updateauthor.Biography;
                existingAuthor.BirthDate = updateauthor.BirthDate;
                existingAuthor.Country = updateauthor.Country;
                existingAuthor.Updated_At = DateTime.Now;

               
                await _context.SaveChangesAsync();

                return "Author update successful";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
          
        }

        public async Task<string> DeleteAuthorAsync(Guid ID)
        {
            var author = await _context.Authors.FindAsync(ID);
            if (author == null)
                return "Author Not found";

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return "Author Delete success";
        }

        public async Task<string> DeleteAuthorAsync()
        {
            var authorToDelete = _context.Authors.OrderBy(a => a.Created_At).FirstOrDefault();
            if (authorToDelete == null)
                return "No authors found";

            _context.Authors.Remove(authorToDelete);
            await _context.SaveChangesAsync();

            return "Author Delete success";
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            return await _context.Authors.ToListAsync();
        }
    }
}
