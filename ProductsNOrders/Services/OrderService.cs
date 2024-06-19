using Microsoft.EntityFrameworkCore;

using ProductsNOrders.Data;
using ProductsNOrders.Enums;
using ProductsNOrders.Exceptions;
using ProductsNOrders.Models;

namespace ProductsNOrders.Services;

internal sealed class OrderService(ApplicationContext context)
{
    private readonly ApplicationContext _context = context;

    public async Task<List<Order>> GetAll() => await _context.Orders.ToListAsync();

    public async Task<Guid> CreateOrderAsync(CancellationToken cancellationToken = default)
    {
        var orderId = Guid.NewGuid();
        await _context.Orders.AddAsync(new Order() { Id = orderId, ProductOrders = [] }, cancellationToken);
        return orderId;
    }

    public async Task AddProductToOrderAsync(Guid orderId, Product product, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders.FindAsync([orderId], cancellationToken)
            ?? throw new ArgumentNullException($"Ошибка в создании запроса с id {orderId}");

        if (order.Status is OrderStatuses.Cancelled) throw new InvalidOperationException($"Невозможно добавить товар в отмененный заказ");
        if (order.Status is OrderStatuses.Confirmed) throw new InvalidOperationException($"Невозможно добавить товар в подтвержденный заказ");

        order.AddProduct(product);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveProductFromOrderAsync(Guid orderId, Guid productId, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders.FindAsync([orderId], cancellationToken)
            ?? throw new ArgumentNullException($"Заказ с id {orderId} не найден");

        if (order.Status is OrderStatuses.Cancelled) throw new InvalidOperationException($"Невозможно убрать товар из отмененного заказа");
        if (order.Status is OrderStatuses.Confirmed) throw new InvalidOperationException($"Невозможно убрать товар из подтвержденного заказа");

        order.RemoveProduct(productId);
        await _context.SaveChangesAsync(cancellationToken);
    }
}