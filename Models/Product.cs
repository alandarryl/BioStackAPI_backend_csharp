namespace BioStockApi.Models;

public class Product
{
    public int Id { get; set; } // L'identifiant unique
    public string Name { get; set; } = string.Empty; // Le nom du réactif ou matériel
    public int Quantity { get; set; } // Combien on en a
    public int Threshold { get; set; } // À partir de quel chiffre on alerte (ex: 5)
    public DateTime ExpirationDate { get; set; } // Date de péremption
}