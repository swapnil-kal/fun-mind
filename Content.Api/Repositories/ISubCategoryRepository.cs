using Content.Api.Entities;

namespace Content.Api.Repositories
{
    public interface ISubCategoryRepository : IRepository<SubCategoryEntity>
    {
        Task<List<SubCategoryEntity>> GetByCategoryId(int categoryId);
    }
}
