using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BioStockApi.Data;
using BioStockApi.Models;

namespace BioStockApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ApiDbContext _context;

    public ProductController(ApiDbContext context)
    {
        _context = context;
    }

    // GET: api/product (Récupérer tous les produits)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }

    // POST: api/product (Ajouter un produit)
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync(); // Sauvegarde physique dans le fichier .db
        
        return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
    }
}