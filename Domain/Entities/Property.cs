using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Domain.Entities
{
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdProperty { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = null!;
        public int Year { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdOwner { get; set; } = null!;

        [BsonIgnoreIfNull]
        public List<string>? PropertyImageIds { get; set; }

        [BsonIgnoreIfNull]
        public List<string>? PropertyTraceIds { get; set; }
    }
}
