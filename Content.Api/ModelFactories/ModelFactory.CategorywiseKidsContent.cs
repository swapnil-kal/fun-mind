using Content.Api.Dto;
using Content.Api.Entities;

namespace Content.Api.ModelFactories
{
    public static partial class ModelFactory
    {
        public static CategoryKidsContentResponse MapCategoryKidsContentFromEntity(KidsContentEntity request)
        {
            var content = new CategoryKidsContentResponse()
            {
                CategoryId = request.CategoryId,
                SubCategoryId = request.SubCategoryId,
                Title = request.SubCategory.Title,
                Description = request.SubCategory.Description
            };
            return content;
        }
    }
}
