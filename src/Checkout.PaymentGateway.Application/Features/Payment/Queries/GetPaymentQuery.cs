using MediatR;

namespace Checkout.PaymentGateway.Application.Features.Payment.Queries;

public class GetPaymentQuery : IRequest<GetPaymentQueryResult>
{
    public Guid Id { get; set; }
}