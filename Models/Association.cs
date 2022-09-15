using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618

public class Association
{
    public int AssociationId { get; set; }
    public int ProductId { get; set; }
    public int CategoryId { get; set; }
    public Product? Product { get; set; }
    public Category? Category { get; set; }
}

