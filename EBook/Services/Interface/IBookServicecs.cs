using EBook.Models;

namespace EBook.Services.Interface
{
    public interface IBookService
    {
        Task<string> AddBookAsync(BookDTO book);
        Task<string> UpdateBookAsync(Guid id, UpdateBook updateBook);
        Task<string> DeleteBookAsync(Guid id);

        Task<List<Book>> SearchBooksByTitleAsync(string title);

        Task<List<Book>> GetBooksByAuthorAsync(Guid authorId);
        Task<List<Book>> GetBooksByGenreAsync(int genreId);

        Task<List<Book>> GetAllBooksAsync();
        //List<Genere> sample();

    }
}
