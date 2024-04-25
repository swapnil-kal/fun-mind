using Content.Api.Dto;
using Content.Api.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Content.Api.Repositories
{
    public class KidsContentRepository : IKidsContentRepository
    {
        private readonly IMongoDatabase _contentDb;

        private readonly IMongoCollection<KidsContentEntity> _kidsContentCollection;
        private readonly IMongoCollection<SubCategoryEntity> _subCategoryCollection;

        public KidsContentRepository(IMongoDatabase contentDb)
        {
            _contentDb = contentDb;
            _kidsContentCollection = _contentDb.GetCollection<KidsContentEntity>(KidsContentEntity.DocumentName);
            _subCategoryCollection = _contentDb.GetCollection<SubCategoryEntity>(SubCategoryEntity.DocumentName);
        }

        public async Task<KidsContentEntity> Create(KidsContentEntity kidsContentEntity)
        {
            await _kidsContentCollection.InsertOneAsync(kidsContentEntity);
            return kidsContentEntity;
        }

        public async Task<bool> Delete(KidsContentEntity kidsContentEntity)
        {
            await _kidsContentCollection.DeleteOneAsync(c => c.Id == kidsContentEntity.Id);
            return true;
        }

        public async Task<List<KidsContentEntity>> GetByCategoryId(int categoryId)
        {
            //var parents = await _kidsContentCollection.FindAsync(c => c.CategoryId == categoryId);            
            //var parentIds = parents.ToList().Select(p => p.Id);

            //var filter = Builders<KidsContentEntity>.Filter.In(p => p.Id, parentIds);
            //var options = new FindOptions<KidsContentEntity, KidsContentEntity> { Projection = Builders<KidsContentEntity>.Projection
            //    .Include(p => p.Title)
            //    .Include(p => p.CategoryId)
            //    .Include(p => p.SubCategoryId)
            //    .Include(p => p.Description)
            //    .Include(p => p.Metadata)
            //    .Include(p => p.ContentDocument)
            //    // .Include(p => p.SubCategory) 
            //};

            //var result = await _kidsContentCollection.FindAsync(filter, options);

            // TODO :: Need to optimize the code
            var kidsContentCursor = await _kidsContentCollection.FindAsync(c => c.CategoryId == categoryId);
            var kidsContentList = await kidsContentCursor.ToListAsync();

            foreach (var content in kidsContentList)
            {
                var subCategory = await _subCategoryCollection.Find(sc => sc.Id == content.SubCategoryId).FirstOrDefaultAsync();
                content.SubCategory = subCategory;
            }

            return kidsContentList;
        }

        public async Task<KidsContentEntity> GetById(string id)
        {
            var result = await _kidsContentCollection.FindAsync(c => c.Id == id);
            return result.FirstOrDefault();
        }

        public async Task<List<KidsContentEntity>> GetBySubCategoryId(string subCategoryId)
        {
            var result = await _kidsContentCollection.FindAsync(c => c.SubCategoryId == subCategoryId);
            return result.ToList();
        }

        public async Task<KidsContentEntity> Update(KidsContentEntity kidsContentEntity)
        {
            await _kidsContentCollection.UpdateOneAsync(c => c.Id == kidsContentEntity.Id, Builders<KidsContentEntity>.Update
               .Set(c => c.CategoryId, kidsContentEntity.CategoryId)
               .Set(c => c.SubCategoryId, kidsContentEntity.SubCategoryId)
               .Set(c => c.Title, kidsContentEntity.Title)
               .Set(c => c.Description, kidsContentEntity.Description)
               .Set(c => c.Metadata, kidsContentEntity.Metadata)
               .Set(c => c.ContentDocument, kidsContentEntity.ContentDocument));

            return kidsContentEntity;
        }
        public async Task<FileEntity> GetContentDocument(string contentId, string documentId)
        {
            var result = await _kidsContentCollection.FindAsync(c => c.Id == contentId);

            var contentDocument = result.FirstOrDefault().ContentDocument;
            if (contentDocument == null)
            {
                return null;
            }

            var imageFile = contentDocument.ImageFiles.Where(x => x.DocumentId == documentId).FirstOrDefault();
            if (imageFile != null)
            {
                return new FileEntity
                {
                    DocumentId = imageFile.DocumentId,
                    FileName = imageFile.FileName,
                    FilePath = imageFile.FilePath
                };
            }

            var videoFile = contentDocument.VideoFiles.Where(x => x.DocumentId == documentId).FirstOrDefault();
            if (videoFile != null)
            {
                return new FileEntity
                {
                    DocumentId = videoFile.DocumentId,
                    FileName = videoFile.FileName,
                    FilePath = videoFile.FilePath
                };
            }

            var audioFile = contentDocument.AudioFiles.Where(x => x.DocumentId == documentId).FirstOrDefault();
            if (audioFile != null)
            {
                return new FileEntity
                {
                    DocumentId = audioFile.DocumentId,
                    FileName = audioFile.FileName,
                    FilePath = audioFile.FilePath
                };
            }

            return null;
        }
    }
}
