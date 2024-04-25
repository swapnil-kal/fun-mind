using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Content.Api.Entities
{
    public class KidsContentEntity : BaseEntity
    {
        public static readonly string DocumentName = "KidsContent";

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }

        public int CategoryId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string SubCategoryId { get; set; }

        public required string Title { get; set; }

        public string Description { get; set; }

        public string Metadata { get; set; }

        public ContentDocumentEntity ContentDocument { get; set; }       

        public SubCategoryEntity SubCategory { get; set; }
    }

    public class ContentDocumentEntity
    {
        public List<AudioFileEntity> AudioFiles { get; set; }
        public List<VideoFileEntity> VideoFiles { get; set; }
        public List<ImageFileEntity> ImageFiles { get; set; }
    }

    public class FileEntity
    {
        public string DocumentId { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }
    } 

    public class AudioFileEntity : FileEntity
    {        
    }

    public class VideoFileEntity : FileEntity
    {
    }

    public class ImageFileEntity : FileEntity
    {
    }
}
