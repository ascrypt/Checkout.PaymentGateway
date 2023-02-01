namespace Checkout.PaymentGateway.Application.Features.Payment.Queries;

public class GetPaymentQueryResult
{
    public Guid Id { get; set; }
    public string Result { get; set; }
    public string CardNumber { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
}