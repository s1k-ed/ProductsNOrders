using Carter;

using Microsoft.AspNetCore.Mvc;

using ProductsNOrders.Enums;
using ProductsNOrders.Models;
using ProductsNOrders.Services;

namespace ProductsNOrders.Apis;

public sealed class OrderApi : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("orders");

        group.MapGet("all", async ([FromServices] OrderService service) =>
        {
            return Results.Ok(await service.GetAll());
        });
        group.MapPost("create", async ([FromBody] CreateOrderDto dto, [FromServices] OrderService service) =>
        {
            if (dto.Amount < 1) return Results.BadRequest("Количество не может быть меньше 1");

            await service.CreateOrderAsync(dto.ProductId, dto.Amount);

            return Results.Created();
        });
        group.MapDelete("{orderId:guid}/delete", async (Guid orderId, [FromServices] OrderService service) =>
        {
            await service.DeleteOrderAsync(orderId);

            return Results.NoContent();
        });
        group.MapPost("{orderId:guid}/add_product", async (Guid orderId, [FromBody] AddProductToOrderDto dto, [FromServices] OrderService service) =>
        {
            if (dto.Amount < 1) return Results.BadRequest("Количество не может быть меньше 1");

            await service.AddProductToOrderAsync(orderId, dto.ProductId, dto.Amount);

            return Results.Created();
        });
        group.MapDelete("{orderId:guid}/remove_product", async (Guid orderId, [FromBody] RemoveProductFromOrderDto dto, [FromServices] OrderService service) =>
        {
            await service.RemoveProductFromOrderAsync(orderId, dto.ProductId);

            return Results.Created();
        });
        group.MapPost("{orderId:guid}/change_status", async (Guid orderId, [FromBody] ChangeOrderStatusDto dto, [FromServices] OrderService service) =>
        {
            if (!Enum.TryParse<OrderStatuses>(dto.Status, true, out var parsedStatus))
                return Results.BadRequest("Невозможно преобразовать значение статуса заказа");

            await service.ChangeOrderStatusAsync(orderId, parsedStatus);
            return Results.Created();
        });
    }
}