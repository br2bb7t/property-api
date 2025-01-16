using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PropertyAPI.Core.Entities
{
    public class PropertyImage
    {
        [BsonId]
        public ObjectId IdPropertyImage { get; set; }
        public ObjectId IdProperty { get; set; }
        public string File { get; set; }
        public bool Enabled { get; set; }
    }
}
