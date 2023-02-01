namespace Checkout.PaymentGateway.Domain.Enums;

public enum PaymentResult
{
    Processing = 0,
    Successful = 1,
    Failed = 2,
    Duplicate = 3,
}