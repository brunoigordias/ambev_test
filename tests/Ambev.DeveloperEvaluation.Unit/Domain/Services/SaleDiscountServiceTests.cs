using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Unit.Domain.Services.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Services;

/// <summary>
/// Contains unit tests for the SaleDiscountService class.
/// Tests cover discount calculation, quantity validation, and business rule enforcement.
/// </summary>
public class SaleDiscountServiceTests
{
    private readonly IDiscountRuleRepository _discountRuleRepository;
    private readonly SaleDiscountService _service;

    /// <summary>
    /// Initializes a new instance of the SaleDiscountServiceTests class.
    /// Sets up the test dependencies and creates the service instance.
    /// </summary>
    public SaleDiscountServiceTests()
    {
        _discountRuleRepository = Substitute.For<IDiscountRuleRepository>();
        _service = new SaleDiscountService(_discountRuleRepository);
    }

    #region IsQuantityValid Tests

    /// <summary>
    /// Tests that a valid quantity within the allowed range returns true.
    /// </summary>
    [Fact(DisplayName = "Given valid quantity When validating Then should return true")]
    public void IsQuantityValid_ValidQuantity_ReturnsTrue()
    {
        // Arrange
        var quantity = SaleDiscountServiceTestData.GenerateValidQuantity();

        // Act
        var result = _service.IsQuantityValid(quantity);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that the minimum allowed quantity returns true.
    /// </summary>
    [Fact(DisplayName = "Given minimum allowed quantity When validating Then should return true")]
    public void IsQuantityValid_MinimumQuantity_ReturnsTrue()
    {
        // Arrange
        var quantity = SaleBusinessRules.MIN_QUANTITY;

        // Act
        var result = _service.IsQuantityValid(quantity);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that the maximum allowed quantity returns true.
    /// </summary>
    [Fact(DisplayName = "Given maximum allowed quantity When validating Then should return true")]
    public void IsQuantityValid_MaximumQuantity_ReturnsTrue()
    {
        // Arrange
        var quantity = SaleBusinessRules.MAX_QUANTITY_ALLOWED;

        // Act
        var result = _service.IsQuantityValid(quantity);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that a quantity below the minimum returns false.
    /// </summary>
    [Fact(DisplayName = "Given quantity below minimum When validating Then should return false")]
    public void IsQuantityValid_QuantityBelowMinimum_ReturnsFalse()
    {
        // Arrange
        var quantity = SaleDiscountServiceTestData.GenerateQuantityBelowMinimum();

        // Act
        var result = _service.IsQuantityValid(quantity);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that a quantity above the maximum returns false.
    /// </summary>
    [Fact(DisplayName = "Given quantity above maximum When validating Then should return false")]
    public void IsQuantityValid_QuantityAboveMaximum_ReturnsFalse()
    {
        // Arrange
        var quantity = SaleDiscountServiceTestData.GenerateQuantityAboveMaximum();

        // Act
        var result = _service.IsQuantityValid(quantity);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region CalculateDiscountPercentageAsync Tests

    /// <summary>
    /// Tests that a valid quantity with an applicable discount rule returns the correct percentage.
    /// </summary>
    [Fact(DisplayName = "Given valid quantity with applicable rule When calculating discount Then should return correct percentage")]
    public async Task CalculateDiscountPercentageAsync_ValidQuantityWithRule_ReturnsDiscountPercentage()
    {
        // Arrange
        var quantity = 5;
        var discountRule = SaleDiscountServiceTestData.GenerateLowTierDiscountRule();
        _discountRuleRepository.GetApplicableRuleAsync(quantity, Arg.Any<CancellationToken>())
            .Returns(discountRule);

        // Act
        var result = await _service.CalculateDiscountPercentageAsync(quantity);

        // Assert
        result.Should().Be(SaleBusinessRules.LOW_DISCOUNT_PERCENTAGE);
        await _discountRuleRepository.Received(1).GetApplicableRuleAsync(quantity, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a valid quantity without an applicable rule returns zero.
    /// </summary>
    [Fact(DisplayName = "Given valid quantity without applicable rule When calculating discount Then should return zero")]
    public async Task CalculateDiscountPercentageAsync_ValidQuantityWithoutRule_ReturnsZero()
    {
        // Arrange
        var quantity = 3; // Below discount threshold
        _discountRuleRepository.GetApplicableRuleAsync(quantity, Arg.Any<CancellationToken>())
            .Returns((DiscountRule?)null);

        // Act
        var result = await _service.CalculateDiscountPercentageAsync(quantity);

        // Assert
        result.Should().Be(0);
        await _discountRuleRepository.Received(1).GetApplicableRuleAsync(quantity, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a quantity above maximum throws DomainException.
    /// </summary>
    [Fact(DisplayName = "Given quantity above maximum When calculating discount Then should throw DomainException")]
    public async Task CalculateDiscountPercentageAsync_QuantityAboveMaximum_ThrowsDomainException()
    {
        // Arrange
        var quantity = SaleDiscountServiceTestData.GenerateQuantityAboveMaximum();

        // Act
        var act = async () => await _service.CalculateDiscountPercentageAsync(quantity);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage($"It's not possible to sell above {SaleBusinessRules.MAX_QUANTITY_ALLOWED} identical items.");
        await _discountRuleRepository.DidNotReceive().GetApplicableRuleAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a quantity below minimum throws DomainException.
    /// </summary>
    [Fact(DisplayName = "Given quantity below minimum When calculating discount Then should throw DomainException")]
    public async Task CalculateDiscountPercentageAsync_QuantityBelowMinimum_ThrowsDomainException()
    {
        // Arrange
        var quantity = 0;

        // Act
        var act = async () => await _service.CalculateDiscountPercentageAsync(quantity);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage($"It's not possible to sell above {SaleBusinessRules.MAX_QUANTITY_ALLOWED} identical items.");
        await _discountRuleRepository.DidNotReceive().GetApplicableRuleAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that high tier discount rule returns correct percentage.
    /// </summary>
    [Fact(DisplayName = "Given quantity in high tier When calculating discount Then should return high discount percentage")]
    public async Task CalculateDiscountPercentageAsync_HighTierQuantity_ReturnsHighDiscountPercentage()
    {
        // Arrange
        var quantity = 15;
        var discountRule = SaleDiscountServiceTestData.GenerateHighTierDiscountRule();
        _discountRuleRepository.GetApplicableRuleAsync(quantity, Arg.Any<CancellationToken>())
            .Returns(discountRule);

        // Act
        var result = await _service.CalculateDiscountPercentageAsync(quantity);

        // Assert
        result.Should().Be(SaleBusinessRules.HIGH_DISCOUNT_PERCENTAGE);
    }

    #endregion

    #region CalculateDiscountAmountAsync Tests

    /// <summary>
    /// Tests that discount amount is calculated correctly with a discount rule.
    /// </summary>
    [Fact(DisplayName = "Given quantity and price with discount When calculating discount amount Then should return correct amount")]
    public async Task CalculateDiscountAmountAsync_WithDiscount_ReturnsCorrectAmount()
    {
        // Arrange
        var quantity = 5;
        var unitPrice = 100m;
        var discountPercentage = 10m;
        var expectedDiscount = (quantity * unitPrice) * (discountPercentage / 100); // 50

        var discountRule = SaleDiscountServiceTestData.GenerateCustomDiscountRule(4, 9, discountPercentage);
        _discountRuleRepository.GetApplicableRuleAsync(quantity, Arg.Any<CancellationToken>())
            .Returns(discountRule);

        // Act
        var result = await _service.CalculateDiscountAmountAsync(quantity, unitPrice);

        // Assert
        result.Should().Be(expectedDiscount);
    }

    /// <summary>
    /// Tests that discount amount returns zero when no discount rule applies.
    /// </summary>
    [Fact(DisplayName = "Given quantity and price without discount When calculating discount amount Then should return zero")]
    public async Task CalculateDiscountAmountAsync_WithoutDiscount_ReturnsZero()
    {
        // Arrange
        var quantity = 2;
        var unitPrice = 100m;

        _discountRuleRepository.GetApplicableRuleAsync(quantity, Arg.Any<CancellationToken>())
            .Returns((DiscountRule?)null);

        // Act
        var result = await _service.CalculateDiscountAmountAsync(quantity, unitPrice);

        // Assert
        result.Should().Be(0);
    }

    /// <summary>
    /// Tests that discount amount calculation throws exception for invalid quantity.
    /// </summary>
    [Fact(DisplayName = "Given invalid quantity When calculating discount amount Then should throw DomainException")]
    public async Task CalculateDiscountAmountAsync_InvalidQuantity_ThrowsDomainException()
    {
        // Arrange
        var quantity = SaleDiscountServiceTestData.GenerateQuantityAboveMaximum();
        var unitPrice = 100m;

        // Act
        var act = async () => await _service.CalculateDiscountAmountAsync(quantity, unitPrice);

        // Assert
        await act.Should().ThrowAsync<DomainException>();
    }

    /// <summary>
    /// Tests that discount amount is calculated correctly with high tier discount.
    /// </summary>
    [Fact(DisplayName = "Given high tier discount When calculating discount amount Then should return correct amount")]
    public async Task CalculateDiscountAmountAsync_HighTierDiscount_ReturnsCorrectAmount()
    {
        // Arrange
        var quantity = 10;
        var unitPrice = 100m;
        var discountPercentage = 20m;
        var expectedDiscount = (quantity * unitPrice) * (discountPercentage / 100); // 200

        var discountRule = SaleDiscountServiceTestData.GenerateHighTierDiscountRule();
        _discountRuleRepository.GetApplicableRuleAsync(quantity, Arg.Any<CancellationToken>())
            .Returns(discountRule);

        // Act
        var result = await _service.CalculateDiscountAmountAsync(quantity, unitPrice);

        // Assert
        result.Should().Be(expectedDiscount);
    }

    #endregion

    #region CalculateTotalAmountAsync Tests

    /// <summary>
    /// Tests that total amount is calculated correctly after discount.
    /// </summary>
    [Fact(DisplayName = "Given quantity and price with discount When calculating total amount Then should return correct total after discount")]
    public async Task CalculateTotalAmountAsync_WithDiscount_ReturnsCorrectTotal()
    {
        // Arrange
        var quantity = 5;
        var unitPrice = 100m;
        var discountPercentage = 10m;
        var subtotal = quantity * unitPrice; // 500
        var discountAmount = subtotal * (discountPercentage / 100); // 50
        var expectedTotal = subtotal - discountAmount; // 450

        var discountRule = SaleDiscountServiceTestData.GenerateCustomDiscountRule(4, 9, discountPercentage);
        _discountRuleRepository.GetApplicableRuleAsync(quantity, Arg.Any<CancellationToken>())
            .Returns(discountRule);

        // Act
        var result = await _service.CalculateTotalAmountAsync(quantity, unitPrice);

        // Assert
        result.Should().Be(expectedTotal);
    }

    /// <summary>
    /// Tests that total amount equals subtotal when no discount applies.
    /// </summary>
    [Fact(DisplayName = "Given quantity and price without discount When calculating total amount Then should return subtotal")]
    public async Task CalculateTotalAmountAsync_WithoutDiscount_ReturnsSubtotal()
    {
        // Arrange
        var quantity = 2;
        var unitPrice = 100m;
        var expectedTotal = quantity * unitPrice; // 200

        _discountRuleRepository.GetApplicableRuleAsync(quantity, Arg.Any<CancellationToken>())
            .Returns((DiscountRule?)null);

        // Act
        var result = await _service.CalculateTotalAmountAsync(quantity, unitPrice);

        // Assert
        result.Should().Be(expectedTotal);
    }

    /// <summary>
    /// Tests that total amount calculation throws exception for invalid quantity.
    /// </summary>
    [Fact(DisplayName = "Given invalid quantity When calculating total amount Then should throw DomainException")]
    public async Task CalculateTotalAmountAsync_InvalidQuantity_ThrowsDomainException()
    {
        // Arrange
        var quantity = SaleDiscountServiceTestData.GenerateQuantityAboveMaximum();
        var unitPrice = 100m;

        // Act
        var act = async () => await _service.CalculateTotalAmountAsync(quantity, unitPrice);

        // Assert
        await act.Should().ThrowAsync<DomainException>();
    }

    /// <summary>
    /// Tests that total amount is calculated correctly with high tier discount.
    /// </summary>
    [Fact(DisplayName = "Given high tier discount When calculating total amount Then should return correct total")]
    public async Task CalculateTotalAmountAsync_HighTierDiscount_ReturnsCorrectTotal()
    {
        // Arrange
        var quantity = 10;
        var unitPrice = 100m;
        var discountPercentage = 20m;
        var subtotal = quantity * unitPrice; // 1000
        var discountAmount = subtotal * (discountPercentage / 100); // 200
        var expectedTotal = subtotal - discountAmount; // 800

        var discountRule = SaleDiscountServiceTestData.GenerateHighTierDiscountRule();
        _discountRuleRepository.GetApplicableRuleAsync(quantity, Arg.Any<CancellationToken>())
            .Returns(discountRule);

        // Act
        var result = await _service.CalculateTotalAmountAsync(quantity, unitPrice);

        // Assert
        result.Should().Be(expectedTotal);
    }

    #endregion
}

