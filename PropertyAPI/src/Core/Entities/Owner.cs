using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PropertyAPI.Core.Entities
{
    public class Owner
    {
        [BsonId]
        public ObjectId IdOwner { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Photo { get; set; }
        public DateTime Birthday { get; set; }
    }
}
