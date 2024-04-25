using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Content.Api.Entities
{   
    public class SubCategoryEntity : BaseEntity
    {
        public static readonly string DocumentName = "SubCategory";

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }

        public int CategoryId { get; set; }

        public required string Title { get; set; }
        
        public string Description { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }
    }
}
