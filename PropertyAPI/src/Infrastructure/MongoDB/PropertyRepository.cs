using MongoDB.Bson;
using MongoDB.Driver;
using PropertyAPI.Core.Entities;
using PropertyAPI.Core.Interfaces;
using PropertyAPI.Core.Dtos;


namespace PropertyAPI.Infrastructure.MongoDB
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly IMongoCollection<Property> _properties;
        private readonly IMongoCollection<Owner> _owners;
        private readonly IMongoCollection<PropertyImage> _propertyImages;
        private readonly IMongoCollection<PropertyTrace> _propertyTraces;

        public PropertyRepository(IMongoDatabase database)
        {
            _properties = database.GetCollection<Property>("Properties");
            _owners = database.GetCollection<Owner>("Owners");
            _propertyImages = database.GetCollection<PropertyImage>("PropertyImages");
            _propertyTraces = database.GetCollection<PropertyTrace>("PropertyTraces");
        }

        public async Task<IEnumerable<PropertyDto>> GetPropertiesAsync(string name, string address, decimal? minPrice, decimal? maxPrice)
        {
            var filter = Builders<Property>.Filter.Empty;

            if (!string.IsNullOrEmpty(name))
                filter &= Builders<Property>.Filter.Regex(p => p.Name, new BsonRegularExpression(name, "i"));

            if (!string.IsNullOrEmpty(address))
                filter &= Builders<Property>.Filter.Regex(p => p.Address, new BsonRegularExpression(address, "i"));

            if (minPrice.HasValue)
                filter &= Builders<Property>.Filter.Gte(p => p.Price, minPrice.Value);

            if (maxPrice.HasValue)
                filter &= Builders<Property>.Filter.Lte(p => p.Price, maxPrice.Value);

            var properties = await _properties.Find(filter).ToListAsync();

            // Obtención de los IdOwner e IdProperty de las propiedades
            var ownerIds = properties.Select(p => p.IdOwner).Distinct().ToList();
            var propertyIds = properties.Select(p => p.IdProperty).Distinct().ToList();

            // Consultas paralelas para obtener los Owners y PropertyImages
            var ownersTask = _owners.Find(o => ownerIds.Contains(o.IdOwner)).ToListAsync();
            var propertyImagesTask = _propertyImages.Find(pi => propertyIds.Contains(pi.IdProperty)).ToListAsync();
            var propertyTracesTask = _propertyTraces.Find(pt => propertyIds.Contains(pt.IdProperty)).ToListAsync();

            // Esperamos que ambas consultas terminen
            await Task.WhenAll(ownersTask, propertyImagesTask, propertyTracesTask);

            var owners = ownersTask.Result;
            var propertyImages = propertyImagesTask.Result;
            var propertyTraces = propertyTracesTask.Result;

            var propertyDtos = properties.Select(p =>
            {
                var owner = owners.FirstOrDefault(o => o.IdOwner == p.IdOwner);
                var propertyImage = propertyImages.FirstOrDefault(pi => pi.IdProperty == p.IdProperty);
                var propertyTrace = propertyTraces.FirstOrDefault(pt => pt.IdProperty == p.IdProperty);

                return new PropertyDto
                {
                    IdOwner = p.IdOwner.ToString(),
                    OwnerName = owner?.Name ?? "Desconocido",
                    OwnerAddress = owner?.Address ?? "No disponible",
                    PropertyName = p.Name,
                    PropertyAddress = p.Address,
                    PropertyPrice = p.Price,
                    PropertyImage = propertyImage?.File ?? "Imagen no disponible",
                    PropertyTraceDate = propertyTrace?.DateSale.ToString("yyyy-MM-dd HH:mm:ss") ?? "No disponible",
                    PropertyTraceName = propertyTrace?.Name ?? "No disponible",
                    PropertyTraceValue = propertyTrace?.Value.ToString() ?? "No disponible",
                    PropertyTraceTax = propertyTrace?.Tax.ToString() ?? "No disponible"
                };
            }).ToList();

            return propertyDtos;
        }

    }
}
