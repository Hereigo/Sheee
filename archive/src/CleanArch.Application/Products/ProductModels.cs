namespace CleanArch.Application.Products;

public sealed record CreateProductRequest(string Name, decimal Price);

public sealed record ProductResponse(Guid Id, string Name, decimal Price);
