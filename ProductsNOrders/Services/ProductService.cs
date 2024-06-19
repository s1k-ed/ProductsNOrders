using Microsoft.EntityFrameworkCore;

using ProductsNOrders.Data;
using ProductsNOrders.Exceptions;
using ProductsNOrders.Models;

namespace ProductsNOrders.Services;

internal sealed class ProductService(ApplicationContext context)
{
    private readonly ApplicationContext _context = context;

    public async Task<List<Product>> GetAll() => await _context.Products.ToListAsync();

    public async Task AddProductAsync(string name, double price, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(new Product() { Id = Guid.NewGuid(), Name = name, Price = price }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FindAsync([productId], cancellationToken)
            ?? throw new NotFoundException($"Товар с id ({productId}) не найден");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
    }
}