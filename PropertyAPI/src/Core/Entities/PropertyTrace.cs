
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PropertyAPI.Core.Entities
{
    public class PropertyTrace
    {
        [BsonId]
        public ObjectId IdPropertyTrace { get; set; }
        public DateTime DateSale { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
        public ObjectId IdProperty { get; set; }
    }
}
