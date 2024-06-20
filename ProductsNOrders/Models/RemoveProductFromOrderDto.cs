namespace ProductsNOrders.Models;

public record RemoveProductFromOrderDto
{
    public Guid ProductId { get; set; } = Guid.Empty;
}