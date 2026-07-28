using FluentAssertions;
using Moq;
using SocialMediaAssistant.Core.Entities;
using SocialMediaAssistant.Core.Interfaces;
using SocialMediaAssistant.Core.Services;

namespace SocialMediaAssistant.UnitTests;

public class StockServiceTests
{
    private readonly Mock<IProductRepository> _productRepoMock = new();
    private readonly StockService _sut;

    public StockServiceTests()
    {
        _sut = new StockService(_productRepoMock.Object);
    }

    [Fact]
    public async Task GetStockSummaryAsync_WhenNoProducts_ReturnsNoCatalogMessage()
    {
        _productRepoMock.Setup(r => r.GetBySellerIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Product>());

        var result = await _sut.GetStockSummaryAsync(Guid.NewGuid());

        result.Should().Be("No products in catalog.");
    }

    [Fact]
    public async Task GetStockSummaryAsync_WithProducts_IncludesProductDetails()
    {
        var sellerId = Guid.NewGuid();
        var products = new List<Product>
        {
            new() { SellerId = sellerId, Name = "Black Dress", SKU = "BD-001", Color = "Black", Size = "M", Price = 299.99m, StockCount = 5 }
        };

        _productRepoMock.Setup(r => r.GetBySellerIdAsync(sellerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var result = await _sut.GetStockSummaryAsync(sellerId);

        result.Should().Contain("Black Dress");
        result.Should().Contain("BD-001");
        result.Should().Contain("Black");
    }

    [Fact]
    public async Task FindProductsAsync_DelegatesToRepository()
    {
        var sellerId = Guid.NewGuid();
        _productRepoMock.Setup(r => r.FindAsync(sellerId, "Black", "M", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Product>());

        await _sut.FindProductsAsync(sellerId, "Black", "M");

        _productRepoMock.Verify(r => r.FindAsync(sellerId, "Black", "M", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateStockAsync_DelegatesToRepository()
    {
        var productId = Guid.NewGuid();
        _productRepoMock.Setup(r => r.UpdateStockAsync(productId, 10, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _sut.UpdateStockAsync(productId, 10);

        _productRepoMock.Verify(r => r.UpdateStockAsync(productId, 10, It.IsAny<CancellationToken>()), Times.Once);
    }
}
