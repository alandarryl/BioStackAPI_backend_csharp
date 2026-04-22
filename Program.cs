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
            policy.AllowAnyOrigin() 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// --- 2. CONFIGURATION DU PIPELINE ---

// On active Swagger même en production pour que tu puisses tester ton URL Render
app.UseSwagger();
app.UseSwaggerUI();

// Important : Sur Render, le HTTPS est géré par leur proxy, 
// cette ligne peut parfois causer des boucles de redirection en ligne.
// On peut la commenter ou la laisser si on configure bien Render.
app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.MapControllers(); 

// --- 3. GESTION DU PORT POUR LE DÉPLOIEMENT ---
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");