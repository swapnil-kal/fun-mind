using Microsoft.EntityFrameworkCore;
using User.Api.Entities;

namespace User.Api.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UserDbContext _dbContext;

        public RoleRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Create(RoleEntity role)
        {
            this._dbContext.Roles.Add(role);
            return (await this._dbContext.SaveChangesAsync()) >= 0;
        }

        public async Task Delete(RoleEntity role)
        {
            this._dbContext.Roles.Remove(role);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoleEntity>> GetAllAsync()
        {
            return await this._dbContext.Roles.ToListAsync();
        }

        public async Task<RoleEntity> GetById(int id)
        {
            return await this._dbContext.Roles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> Update(RoleEntity role)
        {
            this._dbContext.Roles.Attach(role);
            this._dbContext.Entry(role).State = EntityState.Modified;

            return (await this._dbContext.SaveChangesAsync()) >= 0;
        }
    }
}
