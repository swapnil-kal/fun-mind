using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Content.Api.Dto
{
    public class KidsContentDto
    {  
        public string Id { get; init; }

        public int CategoryId { get; set; }

        public string SubCategoryId { get; set; }

        public required string Title { get; set; }

        public string Description { get; set; }

        public ContentDocument ContentDocument { get; set; }

        public string Metadata { get; set; }

        public int? CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public int? DeletedBy { get; set; }

        public DateTimeOffset? Deleted { get; set; }

        public SubCategoryDto SubCategoryDto { get; set; }
    }

    public class ContentDocument
    {
        public List<AudioFile> AudioFiles { get; set; }
        public List<VideoFile> VideoFiles { get; set; }
        public List<ImageFile> ImageFiles { get; set; }
    }

    public class FileDto
    {
        public string DocumentId { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

    }

    public class VideoFile : FileDto
    {

    }

    public class AudioFile : FileDto
    {

    }

    public class ImageFile : FileDto
    {

    }
}
