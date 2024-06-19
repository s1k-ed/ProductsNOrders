namespace ProductsNOrders.Models;

public class Product
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = default!;
    public double Price { get; set; } = default!;
}