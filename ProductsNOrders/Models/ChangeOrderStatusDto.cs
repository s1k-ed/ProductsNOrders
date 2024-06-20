namespace ProductsNOrders.Models;

public record ChangeOrderStatusDto
{
    public string Status { get; set; } = default!;
}