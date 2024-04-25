using Content.Api.Entities;
using Content.Api.Repositories;

namespace Content.Api.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<bool> Create(Book book)
        {
            return await _bookRepository.Create(book);
        }

        public async Task<bool> Delete(string bookId)
        {
            return await _bookRepository.Delete(bookId);
        }

        public async Task<Book> GetBookById(string bookId)
        {
            return await _bookRepository.GetBookById(bookId);
        }

        public async Task<IList<Book>> GetBooks()
        {
            return await _bookRepository.GetBooks();
        }

        public async Task<bool> Update(Book book)
        {
            return await _bookRepository.Update(book);
        }
    }
}
