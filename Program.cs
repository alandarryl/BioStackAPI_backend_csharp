using Microsoft.EntityFrameworkCore;
using BioStockApi.Data;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURATION DES SERVICES (Tout ce qui utilise builder.Services) ---

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlite("Data Source=biostock.db"));

// ON AJOUTE LE SERVICE ICI (Avant le Build !)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.AllowAnyOrigin() 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// --- 2. CONSTRUCTION DE L'APPLICATION ---
var app = builder.Build();

// --- 3. CONFIGURATION DU PIPELINE (Tout ce qui utilise app.Use...) ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ON UTILISE LA POLICY ICI
app.UseCors("AllowReactApp");

app.MapControllers(); 

app.Run();