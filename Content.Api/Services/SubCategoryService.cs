using Amazon.Runtime.Internal;
using Content.Api.Dto;
using Content.Api.Entities;
using Content.Api.Exceptions;
using Content.Api.Extensions;
using Content.Api.Repositories;

namespace Content.Api.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IClaimsProvider _claimsProvider;

        public SubCategoryService(
            ISubCategoryRepository subCategoryRepository,
            IClaimsProvider claimsProvider) {
        
            _subCategoryRepository = subCategoryRepository;
            _claimsProvider = claimsProvider;
        }

        public async Task<SubCategoryDto> Create(CreateSubCategoryRequest request)
        {
            var subCategory = new SubCategoryEntity
            {
                Title = request.Title,
                CategoryId = request.CategoryId,
                Description = request.Description,
                CreatedBy = UserClaimsHelper.GetUserId(_claimsProvider.UserIdentity),
                CreatedOn = DateTime.UtcNow
            };

            var result = await _subCategoryRepository.Create(subCategory);
            var subCategoryDto = new SubCategoryDto
            {
                Id = result.Id,
                Title = result.Title,
                CategoryId = result.CategoryId,
                Description = result.Description,
                CreatedBy = result.CreatedBy,
                CreatedOn = result.CreatedOn
            };
            return subCategoryDto;
        }

        public async Task<SubCategoryDto> Update(string id, UpdateSubCategoryRequest request)
        {
            var subCategory = await _subCategoryRepository.GetById(id);

            if(subCategory == null)
            {
                throw new BadRequestException($"The Sub Category with Id {id} was not found.");
            }

            subCategory.Title = request.Title;
            subCategory.CategoryId = request.CategoryId;
            subCategory.Description = request.Description;
            subCategory.UpdatedOn = DateTime.UtcNow;
            subCategory.UpdatedBy = UserClaimsHelper.GetUserId(_claimsProvider.UserIdentity);

            var result = await _subCategoryRepository.Update(subCategory);
            var subCategoryDto = new SubCategoryDto
            {
                Id = result.Id,
                Title = result.Title,
                CategoryId = result.CategoryId,
                Description = result.Description,
                CreatedBy = result.CreatedBy,
                CreatedOn = result.CreatedOn
            };
            return subCategoryDto;
        }

        public async Task<bool> Delete(string id)
        {
            var subCategory = await _subCategoryRepository.GetById(id);
            return await _subCategoryRepository.Delete(subCategory);
        }

        public async Task<List<SubCategoryDto>> GetByCategoryId(int categoryId)
        {
            var subCategories = await _subCategoryRepository.GetByCategoryId(categoryId);
            var subCategoryDto = subCategories.Select(x => new SubCategoryDto
            {
                Id = x.Id,
                Title = x.Title,
                CategoryId = x.CategoryId,
                Description = x.Description,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn
            }).ToList();

            return subCategoryDto;
        }

        public async Task<SubCategoryDto> GetById(string id)
        {
            var subCategory = await _subCategoryRepository.GetById(id);
            var subCategoryDto = new SubCategoryDto
            {
                Id = subCategory.Id,
                Title = subCategory.Title,
                CategoryId = subCategory.CategoryId,
                Description = subCategory.Description,
                CreatedBy = subCategory.CreatedBy,
                CreatedOn = subCategory.CreatedOn
            };

            return subCategoryDto;
        }
    }
}
