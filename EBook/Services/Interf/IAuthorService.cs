using EBook.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EBook.Services.Interf
{
    public interface IAuthorService
    {

        Task<List<Author>> GetAllAuthorsAsync(); 
        Task<Author> GetAuthorByIdAsync(Guid ID);
        Task<string> AddAuthorAsync(AuthorDTO authors);
        Task<string> UpdateAuthorAsync(Guid id, UpdateAuthor updateauthor);
        Task<string> DeleteAuthorAsync(Guid ID);
        Task<string> DeleteAuthorAsync();

    }
}
