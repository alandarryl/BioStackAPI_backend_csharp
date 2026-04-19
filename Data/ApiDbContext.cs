using Microsoft.EntityFrameworkCore;
using BioStockApi.Models; // On importe nos modèles

namespace BioStockApi.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    // C'est ici qu'on dit : "Je veux une table 'Products' basée sur ma classe Product"
    public DbSet<Product> Products { get; set; }
}