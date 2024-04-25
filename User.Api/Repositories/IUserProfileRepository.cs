using User.Api.Entities;

namespace User.Api.Repositories
{
    public interface IUserProfileRepository : IRepository<UserProfileEntity>
    {
        Task<UserProfileEntity> GetByUserIdAsync(int userId);
    }
}
