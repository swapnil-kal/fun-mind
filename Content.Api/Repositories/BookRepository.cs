using Content.Api.Entities;
using MongoDB.Driver;

namespace Content.Api.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IMongoDatabase _contentDb;

        private readonly IMongoCollection<Book> _mongoCollection;

        public BookRepository(IMongoDatabase contentDb)
        {
            _contentDb = contentDb;
            _mongoCollection = _contentDb.GetCollection<Book>(Book.DocumentName);
        }        

        public async Task<bool> Delete(string bookId)
        {
            await _mongoCollection.DeleteOneAsync(c => c.Id == bookId);
            return true;
        }

        public async Task<Book> GetBookById(string bookId)
        {
            var result = await _mongoCollection.FindAsync(c => c.Id == bookId);
            return result.FirstOrDefault();
        }

        public async Task<IList<Book>> GetBooks()
        {
            var result = await _mongoCollection.FindAsync(FilterDefinition<Book>.Empty);
            return result.ToList();
        }

        public async Task<bool> Update(Book book)
        {
            await _mongoCollection.UpdateOneAsync(c => c.Id == book.Id, Builders<Book>.Update
               .Set(c => c.Name, book.Name)
               .Set(c => c.Description, book.Description)
               .Set(c => c.Price, book.Price));
            return true;
        }

        public async Task<bool> Create(Book book)
        {
            await _mongoCollection.InsertOneAsync(book);
            return true;
        }
    }
}
