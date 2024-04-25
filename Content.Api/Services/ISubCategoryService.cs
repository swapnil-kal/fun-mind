using Content.Api.Dto;
using Content.Api.Entities;

namespace Content.Api.Services
{
    public interface ISubCategoryService
    {
        Task<SubCategoryDto> GetById(string id);

        Task<List<SubCategoryDto>> GetByCategoryId(int categoryId);

        Task<SubCategoryDto> Create(CreateSubCategoryRequest subCategory);

        Task<SubCategoryDto> Update(string id, UpdateSubCategoryRequest subCategory);

        Task<bool> Delete(string id);
    }
}
