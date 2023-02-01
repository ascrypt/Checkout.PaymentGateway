using Checkout.PaymentGateway.Domain.Entities;

namespace Checkout.PaymentGateway.Domain.Events;

public class PaymentCreated
{
    public PaymentCreated(Payment payment)
    {
        Payment = payment;
    }

    public Payment Payment { get; }
}