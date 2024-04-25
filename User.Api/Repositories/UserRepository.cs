using Microsoft.EntityFrameworkCore;
using System.Data;
using User.Api.Dto;
using User.Api.Entities;

namespace User.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _dbContext;

        public UserRepository(UserDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<bool> Create(UserEntity user)
        {
            this._dbContext.Users.Add(user);
            return (await this._dbContext.SaveChangesAsync()) >= 0;
        }

        public async Task Delete(UserEntity user)
        {
            this._dbContext.Users.Remove(user);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserEntity>> GetAllAsync()
        {
            return await this._dbContext.Users.Include(x => x.Role).ToListAsync();
        }

        public async Task<UserEntity> GetByEmailAsync(string email)
        {
            return await this._dbContext.Users
                .Include(x => x.Role)
                .Include(x => x.UserProfile).AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Email.Equals(email));
        }

        public async Task<UserEntity> GetById(int id)
        {
            return await this._dbContext.Users.SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<UserEntity> FindUserWithActiveCodeAsync(string email, string activeCode)
        {
            return await this._dbContext.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email.Equals(email) && x.ActiveCode.Equals(activeCode));
        }

        public async Task<bool> Update(UserEntity user)
        {
            this._dbContext.Users.Attach(user);
            this._dbContext.Entry(user).State = EntityState.Modified;

            return (await this._dbContext.SaveChangesAsync()) >= 0;
        }
    }
}
