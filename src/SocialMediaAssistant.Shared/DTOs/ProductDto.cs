namespace SocialMediaAssistant.Shared.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    string SKU,
    string? Color,
    string? Size,
    int StockCount,
    decimal Price,
    string? Description);

public record CreateProductRequest(
    string Name,
    string SKU,
    string? Color,
    string? Size,
    int StockCount,
    decimal Price,
    string? Description);

public record UpdateStockRequest(int NewStockCount);
