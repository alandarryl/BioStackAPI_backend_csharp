using Microsoft.EntityFrameworkCore;
using BioStockApi.Data;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURATION DES SERVICES ---
builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlite("Data Source=biostock.db"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            // AJOUT : .SetIsOriginAllowed(origin => true) est plus robuste que .AllowAnyOrigin() 
            // pour certains navigateurs quand il y a des erreurs serveurs.
            policy.SetIsOriginAllowed(origin => true) 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// --- 2. INITIALISATION DE LA BASE DE DONNÉES (CRUCIAL POUR RENDER) ---
// Ce bloc force la création du fichier .db et des tables au lancement
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApiDbContext>();
        context.Database.EnsureCreated(); // Crée la base si elle n'existe pas
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erreur lors de la création de la base : " + ex.Message);
    }
}

// --- 3. CONFIGURATION DU PIPELINE ---

app.UseSwagger();
app.UseSwaggerUI();

// MODIFICATION : On commente souvent UseHttpsRedirection sur Render 
// car Render gère déjà le certificat SSL avant d'arriver à ton app.
// app.UseHttpsRedirection(); 

app.UseCors("AllowReactApp");

app.MapControllers(); 

// --- 4. GESTION DU PORT ---
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");