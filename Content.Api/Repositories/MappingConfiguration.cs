using Content.Api.Entities;
using MongoDB.Bson.Serialization;

namespace Content.Api.Repositories
{
    public class MappingConfiguration
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<KidsContentEntity>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true); // Ignore extra fields in MongoDB documents

                cm.MapProperty(p => p.SubCategoryId)
                    .SetElementName("SubCategoryId"); // Map SubCategoryId to MongoDB field

                cm.MapProperty(p => p.SubCategory)
                    .SetElementName("SubCategory")
                    .SetIgnoreIfNull(true); // Ignore mapping if property is null
            });

            BsonClassMap.RegisterClassMap<SubCategoryEntity>(cm =>
            {
                cm.AutoMap();

                cm.MapIdProperty(p => p.Id) // Map "Id" property as the Id field in MongoDB
                    .SetElementName("_id"); // Set the MongoDB field name for Id

            });
        }
    }
}
