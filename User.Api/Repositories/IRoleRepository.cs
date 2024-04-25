using User.Api.Entities;

namespace User.Api.Repositories
{
    public interface IRoleRepository : IRepository<RoleEntity>
    {
        Task<IEnumerable<RoleEntity>> GetAllAsync();
    }
}
