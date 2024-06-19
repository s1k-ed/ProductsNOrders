namespace ProductsNOrders.Models;

public record ProductOrder
{
    public Product Product { get; set; } = default!;
    public int Amount { get; set; } = default!;
}