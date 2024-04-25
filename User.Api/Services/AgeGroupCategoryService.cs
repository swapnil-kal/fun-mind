using User.Api.Dto;
using User.Api.Entities;
using User.Api.Exceptions;
using User.Api.Extensions;
using User.Api.Repositories;

namespace User.Api.Services
{
    public class AgeGroupCategoryService : IAgeGroupCategoryService
    {
        private readonly IAgeGroupCategoryRepository _ageGroupCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IClaimsProvider _claimsProvider;

        public AgeGroupCategoryService(
            IAgeGroupCategoryRepository ageGroupCategoryRepository,
            ICategoryRepository categoryRepository,
            IClaimsProvider claimsProvider)
        {
            _ageGroupCategoryRepository = ageGroupCategoryRepository;
            _categoryRepository = categoryRepository;
            _claimsProvider = claimsProvider;
        }

        public async Task<AgeGroupCategoryDto> GetByIdAsync(int id)
        {
            var agCategoryEntity = await _ageGroupCategoryRepository.GetById(id);

            if (agCategoryEntity == null)
                throw new BadRequestException($"The Age Group Category {id} does not exists.");

            return new AgeGroupCategoryDto { 
                Id = agCategoryEntity.Id,
                AgeGroupId = agCategoryEntity.AgeGroupId,
                CategoryId = agCategoryEntity.CategoryId,
                AgeGroup = new AgeGroupDto
                {
                    Id = agCategoryEntity.AgeGroup.Id,
                    Title = agCategoryEntity.AgeGroup.Title
                },
                Category = new CategoryDto
                {
                    Id = agCategoryEntity.Category.Id,
                    Title= agCategoryEntity.Category.Title,
                }
            };
        }

        public async Task<List<AgeGroupCategoryDto>> FindByAgeGroupIdAsync(int ageGroupId)
        {
            var ageGroupEntity = await _ageGroupCategoryRepository.FindByAgeGroupId(ageGroupId);

            if (ageGroupEntity == null)
                throw new BadRequestException($"The Age Group {ageGroupId} does not exists.");

           return ageGroupEntity.Select(x => new AgeGroupCategoryDto
           {
                Id = x.Id,
                AgeGroupId = x.AgeGroupId,
                CategoryId = x.CategoryId,
                AgeGroup = new AgeGroupDto
                {
                    Id = x.AgeGroup.Id,
                    Title = x.AgeGroup.Title
                },
                Category = new CategoryDto
                {
                    Id = x.Category.Id,
                    Title = x.Category.Title
                }
           }).ToList();
        }

        public async Task<List<AgeGroupCategoryDto>> AssignCategories(int ageGroupId, AgeCategoryMappingRequest request)
        {             
            if(request.RemoveCategories != null)
            {
                await _ageGroupCategoryRepository.RemoveAll(ageGroupId, request.RemoveCategories);
            }

            if (request.NewCategories != null)
            {
                var ageGroupCat = await _ageGroupCategoryRepository.FindByAgeGroupId(ageGroupId);
                var categories = await _categoryRepository.GetByIdsAsync(request.NewCategories);
                var ageCatMapping = new List<AgeGroupCategoryEntity>();

                foreach (var category in categories)
                {
                    if (ageGroupCat.Any(x => x.AgeGroupId == ageGroupId && x.CategoryId == category.Id))
                        continue;

                    var ageGroupCatEntity = new AgeGroupCategoryEntity
                    {
                        AgeGroupId = ageGroupId,
                        CategoryId = category.Id,
                        CreatedBy = UserClaimsHelper.GetUserId(_claimsProvider.UserIdentity),
                        CreatedOn = DateTime.UtcNow
                    };
                    ageCatMapping.Add(ageGroupCatEntity);
                }

                if (ageCatMapping.Any())
                {
                    await _ageGroupCategoryRepository.BulkInsert(ageCatMapping);
                }
            }

            var ageCatEntities = await _ageGroupCategoryRepository.FindByAgeGroupId(ageGroupId);
            var result = ageCatEntities.Select(x => new AgeGroupCategoryDto {Id = x.Id, AgeGroupId = x.AgeGroupId, CategoryId = x.CategoryId }).ToList();
            return result;
        }
    }
}
