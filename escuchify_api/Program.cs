using Microsoft.EntityFrameworkCore;
using escuchify_api;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ====================================================================
// 1. CONFIGURACIÓN DE SERVICIOS
// ====================================================================

// A. Agregar soporte para Controladores (Arquitectura solicitada)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Vital para evitar el error 500 por ciclos (Artista -> Discos -> Artista)
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// B. Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// C. Base de Datos (PostgreSQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// D. CORS (Permitir Frontend)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ====================================================================
// 2. MIDDLEWARE (Pipeline HTTP)
// ====================================================================

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Interfaz de prueba
}

app.UseHttpsRedirection();

app.UseAuthorization();

// E. Mapear automáticamente los controladores
app.MapControllers(); 

app.Run();