using Microsoft.EntityFrameworkCore;
using User.Api.Entities;

namespace User.Api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly UserDbContext _dbContext;

        public CategoryRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Create(CategoryEntity categoryEntity)
        {
            this._dbContext.Categories.Add(categoryEntity);
            return (await this._dbContext.SaveChangesAsync()) >= 0;
        }

        public async Task Delete(CategoryEntity categoryEntity)
        {
            this._dbContext.Categories.Remove(categoryEntity);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryEntity>> GetAllAsync()
        {
            return await this._dbContext.Categories.ToListAsync();
        }

        public async Task<CategoryEntity> GetById(int id)
        {
            return await this._dbContext.Categories.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<CategoryEntity>> GetByIdsAsync(List<int> ids)
        {
            return await this._dbContext.Categories.Where(x => ids.Contains(x.Id) && x.Deleted == null).ToListAsync();
        }

        public async Task<bool> Update(CategoryEntity categoryEntity)
        {
            this._dbContext.Categories.Attach(categoryEntity);
            this._dbContext.Entry(categoryEntity).State = EntityState.Modified;

            return (await this._dbContext.SaveChangesAsync()) >= 0;
        }
    }
}
