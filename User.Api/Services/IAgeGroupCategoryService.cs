using User.Api.Dto;

namespace User.Api.Services
{
    public interface IAgeGroupCategoryService
    {
        Task<AgeGroupCategoryDto> GetByIdAsync(int id);

        Task<List<AgeGroupCategoryDto>> FindByAgeGroupIdAsync(int ageGroupId);

        Task<List<AgeGroupCategoryDto>> AssignCategories(int ageGroupId, AgeCategoryMappingRequest request);
    }
}
