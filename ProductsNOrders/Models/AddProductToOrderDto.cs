namespace ProductsNOrders.Models;

public record AddProductToOrderDto
{
    public Guid ProductId { get; set; } = Guid.Empty;
    public int Amount { get; set; } = default!;
}