using Microsoft.EntityFrameworkCore;
using User.Api.Entities;

namespace User.Api.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly UserDbContext _dbContext;

        public UserProfileRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Create(UserProfileEntity userProfileEntity)
        {
            this._dbContext.UserProfiles.Add(userProfileEntity);
            return (await this._dbContext.SaveChangesAsync()) >= 0;
        }

        public async Task Delete(UserProfileEntity userProfileEntity)
        {
            this._dbContext.UserProfiles.Remove(userProfileEntity);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<UserProfileEntity> GetById(int id)
        {
            return await this._dbContext.UserProfiles.SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<UserProfileEntity> GetByUserIdAsync(int userId)
        {
            return await this._dbContext.UserProfiles
                .Include(x => x.User)
                .Include(x => x.AgeGroup)
                .FirstOrDefaultAsync(x => x.UserId.Equals(userId));
        }

        public async Task<bool> Update(UserProfileEntity userProfileEntity)
        {
            this._dbContext.UserProfiles.Attach(userProfileEntity);
            this._dbContext.Entry(userProfileEntity).State = EntityState.Modified;

            return (await this._dbContext.SaveChangesAsync()) >= 0;
        }
    }
}
