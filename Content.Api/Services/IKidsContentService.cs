using Content.Api.Dto;

namespace Content.Api.Services
{
    public interface IKidsContentService
    {
        Task<KidsContentDto> GetById(string id);

        Task<List<CategoryKidsContentResponse>> GetByCategoryId(int categoryId);

        Task<List<KidsContentDto>> GetBySubCategoryId(string subCategoryId);

        Task<KidsContentDto> Create(CreateKidsContentRequest subCategory);

        Task<KidsContentDto> Update(string id, UpdateKidsContentRequest subCategory);

        Task<bool> Delete(string id);

        Task<Tuple<string, string, MemoryStream>> GetContentDocument(string contentId, string documentId);
    }
}
