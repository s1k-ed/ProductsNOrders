namespace ProductsNOrders.Exceptions;

public sealed class NotFoundException(string message) : Exception(message);