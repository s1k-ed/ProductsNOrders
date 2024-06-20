namespace ProductsNOrders.Models;

public record ProductDto
{
    public string Name { get; set; } = default!;
    public double Price { get; set; } = default!;
}