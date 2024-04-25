using User.Api.Entities;

namespace User.Api.Repositories
{
    public interface ICategoryRepository : IRepository<CategoryEntity>
    {
        Task<IEnumerable<CategoryEntity>> GetAllAsync();

        Task<IEnumerable<CategoryEntity>> GetByIdsAsync(List<int> ids);
    }
}
