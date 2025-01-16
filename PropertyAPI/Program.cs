using MongoDB.Driver;
using PropertyAPI.Infrastructure.MongoDB;
using PropertyAPI.Application.UseCases;
using PropertyAPI.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Habilitar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Permitir cualquier origen
              .AllowAnyMethod()  // Permitir cualquier método HTTP
              .AllowAnyHeader(); // Permitir cualquier cabecera
    });
});


// Agregar controladores
builder.Services.AddControllers();

// Configurar MongoDB
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    // Se obtiene la cadena de conexión desde el archivo de configuración
    var connectionString = builder.Configuration.GetConnectionString("MongoDb") ?? "mongodb://localhost:27017";
    return new MongoClient(connectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<IMongoClient>();
    return client.GetDatabase("PropertyDB");
});

// Agregar datos de prueba
builder.Services.AddSingleton<MongoDataSeeder>();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
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

app.UseCors("AllowAll");  // Usar la política de CORS definida

app.UseSwagger();  // Habilitar el middleware de Swagger
app.UseSwaggerUI();  // Habilitar la interfaz de usuario de Swagger

// Redirigir al Swagger
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

// Inicializar la base de datos
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<MongoDataSeeder>();
    await seeder.SeedAsync();
}

// Configurar las rutas y controladores
app.MapControllers();

app.Run();
