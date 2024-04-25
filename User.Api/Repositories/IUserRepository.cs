using User.Api.Entities;

namespace User.Api.Repositories
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<UserEntity> GetByEmailAsync(string email);

        Task<IEnumerable<UserEntity>> GetAllAsync();

        Task<UserEntity> FindUserWithActiveCodeAsync(string email, string activeCode);
    }
}
