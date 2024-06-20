using Microsoft.EntityFrameworkCore;

using ProductsNOrders.Data;
using ProductsNOrders.Enums;
using ProductsNOrders.Exceptions;
using ProductsNOrders.Models;

namespace ProductsNOrders.Services;

internal sealed class OrderService(ApplicationContext context)
{
    private readonly ApplicationContext _context = context;

    public async Task<List<Order>> GetAll() => await _context.Orders.Include(x => x.ProductOrders).ToListAsync();

    public async Task CreateOrderAsync(Guid productId, int amount, CancellationToken cancellationToken = default)
    {
        _ = await _context.Products.FindAsync([productId], cancellationToken)
            ?? throw new NotFoundException($"Товар с id ({productId}) не найден");

        var order = new Order() { Id = Guid.NewGuid(), ProductOrders = [], Status = OrderStatuses.Processing };
        order.AddProduct(productId, amount);

        await _context.Orders.AddAsync(order, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddProductToOrderAsync(Guid orderId, Guid productId, int amount, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .Include(x => x.ProductOrders)
            .FirstOrDefaultAsync(x => x.Id.Equals(orderId), cancellationToken)
                ?? throw new NotFoundException($"Заказ с id {orderId} не найден");

        var product = await _context.Products.FindAsync(productId, cancellationToken)
            ?? throw new NotFoundException($"Товар с id {productId} не найден");

        if (order.Status is OrderStatuses.Cancelled)
            throw new InvalidOperationException($"Невозможно добавить товар в отмененный заказ");
        if (order.Status is OrderStatuses.Confirmed)
            throw new InvalidOperationException($"Невозможно добавить товар в подтвержденный заказ");

        order.AddProduct(productId, amount);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangeOrderStatusAsync(Guid orderId, OrderStatuses status, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders.FindAsync([orderId], cancellationToken)
            ?? throw new NotFoundException($"Заказ с id {orderId} не найден");

        order.Status = status;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders.FindAsync([orderId], cancellationToken)
            ?? throw new NotFoundException($"Заказ с id {orderId} не найден");

        _context.Remove(order);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveProductFromOrderAsync(Guid orderId, Guid productId, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .Include(x => x.ProductOrders)
            .FirstOrDefaultAsync(x => x.Id.Equals(orderId), cancellationToken)
                ?? throw new ArgumentNullException($"Заказ с id {orderId} не найден");

        if (order.Status is OrderStatuses.Cancelled) throw new InvalidOperationException($"Невозможно убрать товар из отмененного заказа");
        if (order.Status is OrderStatuses.Confirmed) throw new InvalidOperationException($"Невозможно убрать товар из подтвержденного заказа");

        order.RemoveProduct(productId);

        if (order.ProductOrders.Count == 0) _context.Remove(order);

        await _context.SaveChangesAsync(cancellationToken);
    }
}