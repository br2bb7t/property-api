using MongoDB.Driver;
using PropertyAPI.Core.Entities;

public class MongoDataSeeder
{
    private readonly IMongoCollection<Owner> _owners;
    private readonly IMongoCollection<Property> _properties;
    private readonly IMongoCollection<PropertyImage> _propertyImages;
    private readonly IMongoCollection<PropertyTrace> _propertyTraces;

    public MongoDataSeeder(IMongoDatabase database)
    {
        _owners = database.GetCollection<Owner>("Owners");
        _properties = database.GetCollection<Property>("Properties");
        _propertyImages = database.GetCollection<PropertyImage>("PropertyImages");
        _propertyTraces = database.GetCollection<PropertyTrace>("PropertyTraces");
    }

    public async Task SeedAsync()
    {
        // Insertar propietarios de prueba si la colección está vacía
        if (await _owners.CountDocumentsAsync(FilterDefinition<Owner>.Empty) == 0)
        {
            // Crear un propietario de prueba
            var owner = new Owner
            {
                Name = "Juan Pérez",
                Address = "Calle Ficticia 123",
                Birthday = DateTime.Parse("1985-05-20"),
                Photo = "photo_url_example",
            };

            // Insertar el propietario
            await _owners.InsertOneAsync(owner);

            // Obtener el Id generado automáticamente del propietario insertado
            var ownerId = owner.IdOwner;

            // Insertar una propiedad asociada a este propietario
            var property = new Property
            {
                Name = "Casa en la playa",
                Address = "Playa Ficticia 456",
                Price = 300000,
                CodeInternal = "CP12345",
                Year = 2020,
                IdOwner = ownerId  // Asignar el IdOwner correctamente
            };

            // Insertar la propiedad
            await _properties.InsertOneAsync(property);

            // Obtener el Id de la propiedad recién insertada
            var propertyId = property.IdProperty;

            // Insertar imágenes de prueba relacionadas con esta propiedad
            var propertyImage = new PropertyImage
            {
                IdProperty = propertyId,  // Asociar la imagen a la propiedad
                File = "image_url_example",
                Enabled = true
            };

            // Insertar la imagen
            await _propertyImages.InsertOneAsync(propertyImage);

            // Insertar un trace (historial de la propiedad) de prueba relacionado con la propiedad
            var propertyTrace = new PropertyTrace
            {
                IdProperty = propertyId,  // Asociar el trace a la propiedad
                DateSale = DateTime.UtcNow,
                Name = "Venta de propiedad",
                Value = property.Price,
                Tax = 15.0m
            };

            // Insertar el trace
            await _propertyTraces.InsertOneAsync(propertyTrace);
        }
    }
}