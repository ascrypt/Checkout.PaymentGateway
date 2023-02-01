using Checkout.PaymentGateway.BankSimulator.Enums;

namespace Checkout.PaymentGateway.BankSimulator.Models;

public class BankSimulatorResponse
{
    public BankSimulatorResponse(Guid paymentId, BankPaymentResult result)
    {
        PaymentId = paymentId;
        Result = result;
    }

    public Guid PaymentId { get; }
    public BankPaymentResult Result { get; }
}