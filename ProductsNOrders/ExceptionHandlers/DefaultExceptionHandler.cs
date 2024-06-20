using Microsoft.AspNetCore.Diagnostics;

namespace ProductsNOrders.ExceptionsHandlers;

internal sealed class DefaultExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(exception.Message, cancellationToken);

        return true;
    }
}