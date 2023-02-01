using Checkout.PaymentGateway.Domain.Entities;
using MediatR;

namespace Checkout.PaymentGateway.Application.Features.Payment.Commands;

public class CreatePaymentCommand : IRequest<CreatePaymentCommandResult>
{
    public Guid Id { get; set; }
    public string CardHolderName { get; set; }
    public string CardNumber { get; set; }
    public string MerchantId { get; set; }
    public string CvvCode { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
}