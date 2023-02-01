using Checkout.PaymentGateway.Domain.Enums;

namespace Checkout.PaymentGateway.Application.Features.Payment.Commands;

public class CreatePaymentCommandResult
{
    public Guid PaymentId { get; set; }
    public PaymentResult ResultCode { get; set; }
    public string? Result { get; set; }
}