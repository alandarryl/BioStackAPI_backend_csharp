using Microsoft.EntityFrameworkCore;
using BioStockApi.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. AJOUT DES SERVICES
// On dit à .NET qu'on va utiliser des Controllers
builder.Services.AddControllers(); 

// On configure Swagger (OpenAPI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// On configure la base de données SQLite
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlite("Data Source=biostock.db"));

var app = builder.Build();

// Configuration du CORS pour autoriser React (qui tourne souvent sur le port 5173 ou 3000)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.AllowAnyOrigin() // Plus tard, on mettra l'URL précise de ton front
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// 2. CONFIGURATION DU PIPELINE (Requêtes HTTP)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");

// On dit à l'application de chercher les Controllers pour répondre aux URLs
app.MapControllers(); 

app.Run();

