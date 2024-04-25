using Microsoft.EntityFrameworkCore;
using System.Data;
using User.Api.Entities;

namespace User.Api.Repositories
{
    public class AgeGroupRepository : IAgeGroupRepository
    {
        private readonly UserDbContext _dbContext;

        public AgeGroupRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Create(AgeGroupEntity ageGroupEntity)
        {
            this._dbContext.AgeGroups.Add(ageGroupEntity);
            return (await this._dbContext.SaveChangesAsync()) >= 0;
        }

        public async Task Delete(AgeGroupEntity ageGroupEntity)
        {
            this._dbContext.AgeGroups.Remove(ageGroupEntity);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AgeGroupEntity>> GetAllAsync()
        {
            return await this._dbContext.AgeGroups.ToListAsync();
        }

        public async Task<AgeGroupEntity> GetById(int id)
        {
            return await this._dbContext.AgeGroups.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> Update(AgeGroupEntity ageGroupEntity)
        {
            this._dbContext.AgeGroups.Attach(ageGroupEntity);
            this._dbContext.Entry(ageGroupEntity).State = EntityState.Modified;

            return (await this._dbContext.SaveChangesAsync()) >= 0;
        }
    }
}
