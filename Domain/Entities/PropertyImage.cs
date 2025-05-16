using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Domain.Entities
{
    public class PropertyImage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdPropertyImage { get; set; } = null!;

        public string File { get; set; } = null!;
        public bool Enabled { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdProperty { get; set; } = null!;
    }
}
