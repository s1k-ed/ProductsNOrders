using ProductsNOrders.Enums;
using ProductsNOrders.Exceptions;

namespace ProductsNOrders.Models;

public record Order
{
    public Guid Id { get; set; } = Guid.Empty;
    public List<ProductOrder> ProductOrders { get; set; } = default!;
    public OrderStatuses Status { get; set; } = default!;

    public void RemoveProduct(Guid productId)
    {
        var productOrder = ProductOrders.Find(x => x.Product.Id.Equals(productId))
            ?? throw new NotFoundException($"Товар с id ({productId}) не найден в заказе ({Id})");

        if (productOrder.Amount > 1) productOrder.Amount--;
        else ProductOrders.Remove(productOrder);
    }

    public void AddProduct(Product product)
    {
        var productOrder = ProductOrders.Find(x => x.Product.Id.Equals(product.Id))
            ?? new ProductOrder() { Product = product, Amount = 0 };

        productOrder.Amount++;
    }
}