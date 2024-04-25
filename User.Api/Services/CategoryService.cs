using User.Api.Dto;
using User.Api.Entities;
using User.Api.Exceptions;
using User.Api.Repositories;

namespace User.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(x => new CategoryDto { 
                Id = x.Id,
                Title = x.Title,
                BackgroundColor = x.BackgroundColor,
                FileName = x.FileName,
                FilePath = x.FilePath }).ToList();
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var categoryEntity = await _categoryRepository.GetById(id);
            if (categoryEntity == null)
                throw new BadRequestException($"The Id does not exists.");

            return new CategoryDto { 
                Id = categoryEntity.Id,
                Title = categoryEntity.Title, 
                BackgroundColor = categoryEntity.BackgroundColor,
                FileName = categoryEntity.FileName,
                FilePath = categoryEntity.FilePath
            };
        }
    }
}
