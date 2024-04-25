using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Content.Api.Entities
{
    public class Book
    {
        public static readonly string DocumentName = nameof(Book);

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }
        public required string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
