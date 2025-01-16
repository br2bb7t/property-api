using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PropertyAPI.Core.Entities
{
    public class Property
    {
        [BsonId]
        public ObjectId IdProperty { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string CodeInternal { get; set; }
        public int Year { get; set; }
        public ObjectId IdOwner { get; set; }
    }
}
