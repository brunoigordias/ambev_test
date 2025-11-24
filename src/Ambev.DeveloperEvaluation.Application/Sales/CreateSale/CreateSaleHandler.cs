using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Domain.Events;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleDiscountService _discountService;
    private readonly IMapper _mapper;
    private readonly IBus _bus;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    public CreateSaleHandler(
        ISaleRepository saleRepository,
        ISaleDiscountService discountService,
        IMapper mapper,
        IBus bus)
    {
        _saleRepository = saleRepository;
        _discountService = discountService;
        _mapper = mapper;
        _bus = bus;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Check if sale number already exists
        var existingSale = await _saleRepository.GetBySaleNumberAsync(command.SaleNumber, cancellationToken);
        if (existingSale != null)
            throw new InvalidOperationException($"Sale with number {command.SaleNumber} already exists");

        // Create sale entity
        var sale = new Sale
        {
            SaleNumber = command.SaleNumber,
            SaleDate = command.SaleDate,
            CustomerId = command.CustomerId,
            CustomerName = command.CustomerName,
            BranchId = command.BranchId,
            BranchDescription = command.BranchDescription
        };

        // Add items and calculate discounts
        foreach (var itemDto in command.Items)
        {
            var item = new SaleItem(
                itemDto.ProductId,
                itemDto.ProductDescription,
                itemDto.Quantity,
                itemDto.UnitPrice)
            {
                SaleId = sale.Id
            };

            // Calculate and apply discount using the service
            var discountPercentage = await _discountService.CalculateDiscountPercentageAsync(
                itemDto.Quantity, 
                cancellationToken);
            item.ApplyDiscount(discountPercentage);

            sale.Items.Add(item);
        }

        // Calculate total amount
        sale.CalculateTotalAmount();

        // Validate sale entity
        var saleValidation = sale.Validate();
        if (!saleValidation.IsValid)
        {
            throw new ValidationException(
                saleValidation.Errors.Select(e => new FluentValidation.Results.ValidationFailure("Sale", e.Detail)));
        }

        // Save to database
        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        // Publish SaleCreatedEvent via Rebus
        var saleCreatedEvent = new SaleCreatedEvent(createdSale);
        await _bus.Publish(saleCreatedEvent);

        return _mapper.Map<CreateSaleResult>(createdSale);
    }
}

