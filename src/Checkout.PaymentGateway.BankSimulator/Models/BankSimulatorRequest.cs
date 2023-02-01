namespace Checkout.PaymentGateway.BankSimulator.Models;

public class BankSimulatorRequest
{
    public BankSimulatorRequest()
    {
        
    }
    public BankSimulatorRequest(Guid paymentId, string cardHolderName, string cardNumber, int expirationYear,
        int expirationMonth, decimal amount, string cvv, string currency)
    {
        PaymentId = paymentId;
        CardHolderName = cardHolderName;
        CardNumber = cardNumber;
        ExpirationYear = expirationYear;
        ExpirationMonth = expirationMonth;
        Amount = amount;
        Cvv = cvv;
        Currency = currency;
    }

    public Guid PaymentId { get; set; }
    public string CardHolderName { get; set; }
    public string CardNumber { get; set; }
    public int ExpirationYear { get; set; }
    public int ExpirationMonth { get; set; }
    public decimal Amount { get; set; }
    public string Cvv { get; set; }
    public string Currency { get; set; }
}