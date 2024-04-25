using Content.Api.Entities;

namespace Content.Api.Repositories
{
    public interface IBookRepository
    {
        Task<IList<Book>> GetBooks();

        Task<Book> GetBookById(string bookId);

        Task<bool> Create(Book book);

        Task<bool> Update(Book book);

        Task<bool> Delete(string bookId);
    }
}
