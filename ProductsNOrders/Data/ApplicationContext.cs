using Microsoft.EntityFrameworkCore;

using ProductsNOrders.Models;

namespace ProductsNOrders.Data;

internal sealed class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    internal DbSet<Product> Products { get; set; }
    internal DbSet<Order> Orders { get; set; }
}