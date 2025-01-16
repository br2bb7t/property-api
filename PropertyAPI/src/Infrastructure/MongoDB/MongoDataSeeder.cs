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
            // Datos de prueba para propietarios
            var owners = new List<Owner>
            {
                new Owner
                {
                    Name = "Juan Pérez",
                    Address = "Calle Ficticia 123",
                    Birthday = DateTime.Parse("1985-05-20"),
                    Photo = "photo_url_example_1"
                },
                new Owner
                {
                    Name = "Maria García",
                    Address = "Avenida Real 456",
                    Birthday = DateTime.Parse("1990-08-15"),
                    Photo = "photo_url_example_2"
                },
                new Owner
                {
                    Name = "Carlos López",
                    Address = "Calle Falsa 789",
                    Birthday = DateTime.Parse("1982-11-30"),
                    Photo = "photo_url_example_3"
                },
                new Owner
                {
                    Name = "Ana Torres",
                    Address = "Calle de los Olivos 321",
                    Birthday = DateTime.Parse("1975-02-25"),
                    Photo = "photo_url_example_4"
                },
                new Owner
                {
                    Name = "Luis Fernández",
                    Address = "Calle Nueva 654",
                    Birthday = DateTime.Parse("1988-06-05"),
                    Photo = "photo_url_example_5"
                }
            };

            // Insertar propietarios
            await _owners.InsertManyAsync(owners);

            // Insertar propiedades asociadas a los propietarios
            var properties = new List<Property>
            {
                new Property
                {
                    Name = "Casa en la playa",
                    Address = "Playa Ficticia 456",
                    Price = 300000,
                    CodeInternal = "CP12345",
                    Year = 2020,
                    IdOwner = owners[0].IdOwner
                },
                new Property
                {
                    Name = "Apartamento en la ciudad",
                    Address = "Centro Ciudad 789",
                    Price = 150000,
                    CodeInternal = "AP54321",
                    Year = 2018,
                    IdOwner = owners[1].IdOwner
                },
                new Property
                {
                    Name = "Villa de lujo",
                    Address = "Calle de las Palmeras 123",
                    Price = 500000,
                    CodeInternal = "VL67890",
                    Year = 2022,
                    IdOwner = owners[2].IdOwner
                },
                new Property
                {
                    Name = "Casa rústica",
                    Address = "Campo 101",
                    Price = 200000,
                    CodeInternal = "CR11223",
                    Year = 2015,
                    IdOwner = owners[3].IdOwner
                },
                new Property
                {
                    Name = "Piso moderno",
                    Address = "Zona Industrial 456",
                    Price = 180000,
                    CodeInternal = "PM22334",
                    Year = 2019,
                    IdOwner = owners[4].IdOwner
                }
            };

            // Insertar propiedades
            await _properties.InsertManyAsync(properties);

            // Insertar imágenes y trazas de prueba relacionadas con estas propiedades
            var propertyImages = new List<PropertyImage>
            {
                new PropertyImage
                {
                    IdProperty = properties[0].IdProperty,
                    File = "image_url_example_1",
                    Enabled = true
                },
                new PropertyImage
                {
                    IdProperty = properties[1].IdProperty,
                    File = "image_url_example_2",
                    Enabled = true
                },
                new PropertyImage
                {
                    IdProperty = properties[2].IdProperty,
                    File = "image_url_example_3",
                    Enabled = true
                },
                new PropertyImage
                {
                    IdProperty = properties[3].IdProperty,
                    File = "image_url_example_4",
                    Enabled = true
                },
                new PropertyImage
                {
                    IdProperty = properties[4].IdProperty,
                    File = "image_url_example_5",
                    Enabled = true
                }
            };

            // Insertar imágenes
            await _propertyImages.InsertManyAsync(propertyImages);

            // Insertar trazas de prueba relacionadas con estas propiedades
            var propertyTraces = new List<PropertyTrace>
            {
                new PropertyTrace
                {
                    IdProperty = properties[0].IdProperty,
                    DateSale = DateTime.UtcNow,
                    Name = "Venta de propiedad",
                    Value = properties[0].Price,
                    Tax = 15.0m
                },
                new PropertyTrace
                {
                    IdProperty = properties[1].IdProperty,
                    DateSale = DateTime.UtcNow,
                    Name = "Venta de propiedad",
                    Value = properties[1].Price,
                    Tax = 10.0m
                },
                new PropertyTrace
                {
                    IdProperty = properties[2].IdProperty,
                    DateSale = DateTime.UtcNow,
                    Name = "Venta de propiedad",
                    Value = properties[2].Price,
                    Tax = 20.0m
                },
                new PropertyTrace
                {
                    IdProperty = properties[3].IdProperty,
                    DateSale = DateTime.UtcNow,
                    Name = "Venta de propiedad",
                    Value = properties[3].Price,
                    Tax = 18.0m
                },
                new PropertyTrace
                {
                    IdProperty = properties[4].IdProperty,
                    DateSale = DateTime.UtcNow,
                    Name = "Venta de propiedad",
                    Value = properties[4].Price,
                    Tax = 12.0m
                }
            };

            // Insertar trazas
            await _propertyTraces.InsertManyAsync(propertyTraces);
        }
    }
}
