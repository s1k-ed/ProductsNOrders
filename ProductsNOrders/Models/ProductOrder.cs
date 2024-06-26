namespace ProductsNOrders.Models;

public record ProductOrder
{
    public Guid ProductId { get; set; } = Guid.Empty;
    public Guid OrderId { get; set; } = Guid.Empty;
    public int Amount { get; set; } = default!;
}