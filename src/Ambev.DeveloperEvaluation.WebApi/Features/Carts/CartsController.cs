using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.WebApi.Validation;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCarts;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

/// <summary>
/// Controller for managing cart operations
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CartsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CartsController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CartsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all carts with optional filtering, pagination and ordering
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="size">Page size (default: 10)</param>
    /// <param name="order">Order by expression (e.g., "id desc, userId asc")</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of carts</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetCartsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCarts(
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string? order = null,
        CancellationToken cancellationToken = default)
    {
        // Extract filters from query string
        var filters = new Dictionary<string, string>();
        foreach (var query in HttpContext.Request.Query)
        {
            if (query.Key != "page" && query.Key != "size" && query.Key != "order" && 
                query.Key != "_page" && query.Key != "_size" && query.Key != "_order")
            {
                filters[query.Key] = query.Value.ToString();
            }
        }

        // Support both _page/_size and page/size
        var actualPage = HttpContext.Request.Query.ContainsKey("_page") 
            ? int.TryParse(HttpContext.Request.Query["_page"], out var p) ? p : page 
            : page;
        var actualSize = HttpContext.Request.Query.ContainsKey("_size") 
            ? int.TryParse(HttpContext.Request.Query["_size"], out var s) ? s : size 
            : size;
        var actualOrder = HttpContext.Request.Query.ContainsKey("_order") 
            ? HttpContext.Request.Query["_order"].ToString() 
            : order;

        var getCartsQuery = new GetCartsQuery
        {
            Page = actualPage,
            Size = actualSize,
            Order = actualOrder,
            Filters = filters.Any() ? filters : null
        };

        var response = await _mediator.Send(getCartsQuery, cancellationToken);
        return Ok(_mapper.Map<GetCartsResponse>(response));
    }

    /// <summary>
    /// Creates a new cart
    /// </summary>
    /// <param name="request">The cart creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created cart details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateCartResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCart([FromBody] CreateCartRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateCartCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created($"/api/carts/{response.Id}", _mapper.Map<CreateCartResponse>(response));
    }

    /// <summary>
    /// Retrieves a cart by its ID
    /// </summary>
    /// <param name="id">The unique identifier of the cart</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cart details if found</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetCartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCart([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCartQuery { Id = id };
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(_mapper.Map<GetCartResponse>(response));
    }

    /// <summary>
    /// Updates an existing cart
    /// </summary>
    /// <param name="id">The unique identifier of the cart</param>
    /// <param name="request">The cart update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated cart details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UpdateCartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCart([FromRoute] Guid id, [FromBody] UpdateCartRequest request, CancellationToken cancellationToken)
    {
        var validator = new UpdateCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateCartCommand>(request);
        command.Id = id;
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<UpdateCartResponse>(response));
    }

    /// <summary>
    /// Deletes a cart by its ID
    /// </summary>
    /// <param name="id">The unique identifier of the cart to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success message</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(DeleteCartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCart([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteCartCommand { Id = id };
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(_mapper.Map<DeleteCartResponse>(response));
    }
}

