namespace ProductsNOrders.Models;

public record CreateOrderDto
{
    public Guid ProductId { get; set; } = Guid.Empty;
    public int Amount { get; set; } = default!;
}