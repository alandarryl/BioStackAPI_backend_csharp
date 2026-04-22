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


// DELETE: api/product/5
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteProduct(int id)
{
    var product = await _context.Products.FindAsync(id);
    if (product == null)
    {
        return NotFound(); // Si l'ID n'existe pas
    }

    _context.Products.Remove(product);
    await _context.SaveChangesAsync(); // Applique la suppression en base

    return NoContent(); // Réponse standard pour une suppression réussie (204)
}

// PUT: api/product/5
[HttpPut("{id}")]
public async Task<IActionResult> UpdateQuantity(int id, [FromBody] int newQuantity)
{
    var product = await _context.Products.FindAsync(id);
    if (product == null) return NotFound();

    product.Quantity = newQuantity;
    await _context.SaveChangesAsync();

    return NoContent();
}

}