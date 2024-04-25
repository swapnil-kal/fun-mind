using Amazon.Util.Internal;
using Content.Api.Entities;

namespace Content.Api.Dto
{
    public class CreateKidsContentRequest
    {
        public int CategoryId { get; set; }

        public string SubCategoryId { get; set; }

        public required string Title { get; set; }

        public string Description { get; set; }

        public ContentDocumentRequest ContentDocument { get; set; }       

        public string Metadata { get; set; }
    }

    public class ContentDocumentRequest
    {
        public List<IFormFile> AudioFiles { get; set; }
        public List<IFormFile> VideoFiles { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
    }

}
