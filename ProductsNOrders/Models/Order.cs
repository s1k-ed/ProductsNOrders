using ProductsNOrders.Enums;
using ProductsNOrders.Exceptions;

namespace ProductsNOrders.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.Empty;
    public List<ProductOrder> ProductOrders { get; set; } = default!;
    public OrderStatuses Status { get; set; } = default!;

    public void RemoveProduct(Guid productId)
    {
        var productOrder = ProductOrders.Find(x => x.ProductId.Equals(productId))
            ?? throw new NotFoundException($"Товар с id ({productId}) не найден в заказе ({Id})");

        if (productOrder.Amount > 1) productOrder.Amount--;
        else ProductOrders.Remove(productOrder);
    }

    public void AddProduct(Guid productId, int amount)
    {
        var productOrder = ProductOrders.Find(x => x.ProductId.Equals(productId) && x.OrderId.Equals(Id));

        if (productOrder is null)
        {
            productOrder = new ProductOrder()
            {
                OrderId = Id,
                ProductId = productId,
                Amount = 0
            };

            ProductOrders.Add(productOrder);
        }

        productOrder.Amount += amount;
    }
}