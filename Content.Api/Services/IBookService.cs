using Content.Api.Entities;

namespace Content.Api.Services
{
    public interface IBookService
    {
        Task<IList<Book>> GetBooks();
        
        Task<Book> GetBookById(string bookId);
        
        Task<bool> Create(Book book);
        
        Task<bool> Update(Book book);

        Task<bool> Delete(string bookId);
    }
}
