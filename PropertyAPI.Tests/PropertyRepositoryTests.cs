using MongoDB.Bson;
using MongoDB.Driver;
using Mongo2Go;
using PropertyAPI.Core.Entities;
using PropertyAPI.Infrastructure.MongoDB;

public class PropertyRepositoryTests : IAsyncLifetime
{
    private readonly MongoDbRunner _mongoDbRunner;
    private IMongoDatabase _database;
    private IMongoCollection<Property> _propertyCollection;
    private IMongoCollection<Owner> _ownerCollection;
    private PropertyRepository _repository;

    public PropertyRepositoryTests()
    {
        // Inicializamos Mongo2Go
        _mongoDbRunner = MongoDbRunner.Start();
    }

    public async Task InitializeAsync()
    {
        // Inicializamos las colecciones y el repositorio
        var client = new MongoClient(_mongoDbRunner.ConnectionString);
        _database = client.GetDatabase("test");
        _propertyCollection = _database.GetCollection<Property>("Properties");
        _ownerCollection = _database.GetCollection<Owner>("Owners");

        _repository = new PropertyRepository(_database);
        
        // Insertamos datos de prueba en las colecciones
        var owner = new Owner
        {
            IdOwner = ObjectId.GenerateNewId(),
            Name = "Juan Pérez",
            Address = "Calle Ficticia 123"
        };
        await _ownerCollection.InsertOneAsync(owner);

        var property = new Property
        {
            IdOwner = owner.IdOwner,
            Name = "Casa en la playa",
            Address = "Playa Ficticia 456",
            Price = 300000
        };
        await _propertyCollection.InsertOneAsync(property);
    }

    public async Task DisposeAsync()
    {
        // Limpiar la base de datos
        _mongoDbRunner.Dispose();
    }

    [Fact]
    public async Task GetPropertiesAsync_ReturnsProperties_WithOwner()
    {
        // Act: Llamada al método que estamos probando
        var properties = await _repository.GetPropertiesAsync(null, null, null, null);

        // Assert: Verifica que los datos sean correctos
        var propertyDto = properties.First();
        Assert.NotNull(propertyDto);
        Assert.Equal("Juan Pérez", propertyDto.OwnerName);
        Assert.Equal("Calle Ficticia 123", propertyDto.OwnerAddress);
        Assert.Equal("Casa en la playa", propertyDto.PropertyName);
        Assert.Equal("Playa Ficticia 456", propertyDto.PropertyAddress);
        Assert.Equal(300000, propertyDto.PropertyPrice);
    }
}
