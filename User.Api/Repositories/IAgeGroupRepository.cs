using User.Api.Entities;

namespace User.Api.Repositories
{
    public interface IAgeGroupRepository : IRepository<AgeGroupEntity>
    {
        Task<IEnumerable<AgeGroupEntity>> GetAllAsync();
    }
}
