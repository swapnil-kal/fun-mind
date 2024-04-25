using User.Api.Entities;

namespace User.Api.Repositories
{
    public interface IAgeGroupCategoryRepository : IRepository<AgeGroupCategoryEntity>
    {
        Task<List<AgeGroupCategoryEntity>> BulkInsert(List<AgeGroupCategoryEntity> ageGroupCategories);

        Task<List<AgeGroupCategoryEntity>> FindByAgeGroupId(int ageGroupId);

        Task<bool> RemoveAll(int ageGroupId, List<int> categoryIds);
    }
}
