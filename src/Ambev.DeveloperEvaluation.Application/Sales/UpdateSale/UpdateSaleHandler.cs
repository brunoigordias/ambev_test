using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for processing UpdateSaleCommand requests
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleDiscountService _discountService;
    private readonly IMapper _mapper;

    public UpdateSaleHandler(
        ISaleRepository saleRepository,
        ISaleDiscountService discountService,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _discountService = discountService;
        _mapper = mapper;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        // Update sale properties
        sale.SaleNumber = command.SaleNumber;
        sale.SaleDate = command.SaleDate;
        sale.CustomerId = command.CustomerId;
        sale.CustomerName = command.CustomerName;
        sale.BranchId = command.BranchId;
        sale.BranchDescription = command.BranchDescription;

        // Clear existing items and add new ones
        sale.Items.Clear();
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

            var discountPercentage = await _discountService.CalculateDiscountPercentageAsync(
                itemDto.Quantity,
                cancellationToken);
            item.ApplyDiscount(discountPercentage);

            sale.Items.Add(item);
        }

        sale.CalculateTotalAmount();

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        // TODO: Publish SaleModifiedEvent
        // var saleModifiedEvent = new SaleModifiedEvent(updatedSale);
        // await _mediator.Publish(saleModifiedEvent, cancellationToken);

        return _mapper.Map<UpdateSaleResult>(updatedSale);
    }
}

