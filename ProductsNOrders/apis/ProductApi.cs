using Carter;

using Microsoft.AspNetCore.Mvc;

using ProductsNOrders.Models;
using ProductsNOrders.Services;

namespace ProductsNOrders.Apis;

public sealed class ProductApi : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("products");

        group.MapGet("all", async ([FromServices] ProductService service) =>
        {
            return Results.Ok(await service.GetAll());
        });
        group.MapPost("create", async ([FromBody] ProductDto dto, [FromServices] ProductService service) =>
        {
            await service.AddProductAsync(dto.Name, dto.Price);

            return Results.Created();
        });
        group.MapDelete("{productId:guid}/delete", async (Guid productId, [FromServices] ProductService service) =>
        {
            await service.RemoveProductAsync(productId);

            return Results.NoContent();
        });
    }
}