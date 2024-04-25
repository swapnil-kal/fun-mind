using Content.Api.Entities;
using MongoDB.Driver;
using System.Net;

namespace Content.Api.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly IMongoDatabase _contentDb;

        private readonly IMongoCollection<SubCategoryEntity> _mongoCollection;

        public SubCategoryRepository(IMongoDatabase contentDb)
        {
            _contentDb = contentDb;
            _mongoCollection = _contentDb.GetCollection<SubCategoryEntity>(SubCategoryEntity.DocumentName);
        }

        public async Task<SubCategoryEntity> Create(SubCategoryEntity subCategory)
        {
            await _mongoCollection.InsertOneAsync(subCategory);
            return subCategory;
        }

        public async Task<bool> Delete(SubCategoryEntity subCategory)
        {
            await _mongoCollection.DeleteOneAsync(c => c.Id == subCategory.Id);
            return true;
        }

        public async Task<SubCategoryEntity> GetById(string id)
        {
            var result = await _mongoCollection.FindAsync(c => c.Id == id);
            return result.FirstOrDefault();
        }

        public async Task<List<SubCategoryEntity>> GetByCategoryId(int categoryId)
        {
            var result = await _mongoCollection.FindAsync(c => c.CategoryId == categoryId);
            return result.ToList();
        }

        public async Task<SubCategoryEntity> Update(SubCategoryEntity subCategory)
        {
            await _mongoCollection.UpdateOneAsync(c => c.Id == subCategory.Id, Builders<SubCategoryEntity>.Update
               .Set(c => c.CategoryId, subCategory.CategoryId)
               .Set(c => c.Title, subCategory.Title)
               .Set(c => c.Description, subCategory.Description)
               .Set(c => c.FileName, subCategory.FileName)
               .Set(c => c.FilePath, subCategory.FilePath));

            return subCategory;
        }
    }
}
