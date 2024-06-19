
using Microsoft.AspNetCore.Diagnostics;

using ProductsNOrders.Exceptions;

namespace ProductsNOrders.ExceptionsHandlers;

internal sealed class NotFoundExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException) return false;

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        await httpContext.Response.WriteAsJsonAsync(exception.Message, cancellationToken);

        return true;
    }
}