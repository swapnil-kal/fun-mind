using Content.Api.Entities;

namespace Content.Api.Repositories
{
    public interface IKidsContentRepository : IRepository<KidsContentEntity>
    {
        Task<List<KidsContentEntity>> GetByCategoryId(int categoryId);

        Task<List<KidsContentEntity>> GetBySubCategoryId(string subCategoryId);

        Task<FileEntity> GetContentDocument(string contentId, string documentId);
    }
}
