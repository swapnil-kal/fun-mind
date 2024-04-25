using Microsoft.EntityFrameworkCore;
using User.Api.Entities;

namespace User.Api.Repositories
{
    public class AgeGroupCategoryRepository : IAgeGroupCategoryRepository
    {
        private readonly UserDbContext _dbContext;

        public AgeGroupCategoryRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<bool> Create(AgeGroupCategoryEntity ageGroupCategory)
        {
            this._dbContext.AgeGroupCategories.Add(ageGroupCategory);
            return (await this._dbContext.SaveChangesAsync()) >= 0;
        }

        public async Task<List<AgeGroupCategoryEntity>> BulkInsert(List<AgeGroupCategoryEntity> ageGroupCategories)
        {
            this._dbContext.AgeGroupCategories.AddRange(ageGroupCategories);
            await this._dbContext.SaveChangesAsync();
            return ageGroupCategories;
        }

        public async Task Delete(AgeGroupCategoryEntity ageGroupCategory)
        {
            this._dbContext.AgeGroupCategories.Remove(ageGroupCategory);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<AgeGroupCategoryEntity> GetById(int id)
        {
            return await this._dbContext.AgeGroupCategories
                .Include(x => x.AgeGroup)
                .Include(x => x.Category)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<AgeGroupCategoryEntity>> FindByAgeGroupId(int ageGroupId)
        {
            return await this._dbContext.AgeGroupCategories
                .Include(x => x.AgeGroup)
                .Include(x => x.Category)
                .Where(x => x.AgeGroupId == ageGroupId && x.Deleted == null).ToListAsync();
        }

        public async Task<bool> Update(AgeGroupCategoryEntity ageGroupCategory)
        {
            this._dbContext.AgeGroupCategories.Attach(ageGroupCategory);
            this._dbContext.Entry(ageGroupCategory).State = EntityState.Modified;

            return (await this._dbContext.SaveChangesAsync()) >= 0;
        }

        public async Task<bool> RemoveAll(int ageGroupId, List<int> categoryIds)
        {
            var ageGroupCategories = this._dbContext.AgeGroupCategories.Where(x => x.AgeGroupId == ageGroupId && categoryIds.Contains(x.CategoryId));
            this._dbContext.AgeGroupCategories.RemoveRange(ageGroupCategories);
            return (await this._dbContext.SaveChangesAsync()) >= 0;
        }
    }
}
