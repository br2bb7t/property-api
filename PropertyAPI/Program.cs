using MongoDB.Driver;
using PropertyAPI.Infrastructure.MongoDB;
using PropertyAPI.Application.UseCases;
using PropertyAPI.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores
builder.Services.AddControllers();

// Configurar MongoDB
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    // Se obtiene la cadena de conexión desde el archivo de configuración
    var connectionString = builder.Configuration.GetConnectionString("MongoDb") ?? "mongodb://localhost:27017"; // Definir un valor por defecto
    return new MongoClient(connectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<IMongoClient>();
    return client.GetDatabase("PropertyDB");  // Asegúrate de que este sea el nombre de tu base de datos
});

// Agregar servicio de siembra de datos
builder.Services.AddSingleton<MongoDataSeeder>();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();  // Necesario para la exploración de endpoints
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "PropertyAPI",
        Version = "v1",
        Description = "API para la gestión de propiedades"
    });
});

// Inyectar dependencias para repositorios y casos de uso
builder.Services.AddScoped<IPropertyRepository>(sp => 
    new PropertyRepository(sp.GetRequiredService<IMongoDatabase>()));
builder.Services.AddScoped<GetPropertiesUseCase>();


var app = builder.Build();

// Configurar el middleware de la aplicación
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseSwagger();  // Habilitar el middleware de Swagger
app.UseSwaggerUI();  // Habilitar la interfaz de usuario de Swagger

// Redirigir al Swagger
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

// Ejecutar el servicio de siembra de datos
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<MongoDataSeeder>();
    await seeder.SeedAsync();
}

// Configurar las rutas y controladores
app.MapControllers();

app.Run();
