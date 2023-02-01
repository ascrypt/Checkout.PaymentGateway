using Checkout.PaymentGateway.Api.Authentication;
using Checkout.PaymentGateway.Application.Features.Payment.Commands;
using Checkout.PaymentGateway.Application.Features.Payment.Queries;
using Checkout.PaymentGateway.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Checkout.PaymentGateway.Api.Controllers;

[ApiKey]
[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Get payment details
    /// </summary>
    /// <param name="query"></param>
    /// <returns>GetPaymentQueryResult</returns>
    [HttpGet("{Id}")]
    [EnableRateLimiting("LimiterPolicy")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(GetPaymentQueryResult))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPayment([FromRoute] GetPaymentQuery query)
    {
        var result = await _mediator.Send(query);
        return result is null ? NotFound($"Not found a payment with id {query.Id}") : Ok(result);
    }

    /// <summary>
    ///     Create a payment
    /// </summary>
    /// <param name="command"></param>
    /// <returns>CreatePaymentResult</returns>
    [HttpPost("")]
    [EnableRateLimiting("LimiterPolicy")]
    [Produces("application/json")]
    [ProducesResponseType(201, Type = typeof(CreatePaymentCommandResult))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentCommand command)
    {
        var createPayment = await _mediator.Send(command);
        createPayment.Result = Enum.GetName(typeof(PaymentResult), createPayment.ResultCode);
        if (createPayment.ResultCode == PaymentResult.Duplicate)
        {
            return BadRequest(createPayment);
        }

        return CreatedAtAction("CreatePayment", new { paymentId = createPayment.PaymentId }, createPayment);
    }
}