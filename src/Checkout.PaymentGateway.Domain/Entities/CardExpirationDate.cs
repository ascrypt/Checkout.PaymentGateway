namespace Checkout.PaymentGateway.Domain.Entities;

public class CardExpirationDate
{
    public CardExpirationDate(int month, int year)
    {
        Month = month;
        Year = year;
    }

    public int Month { get; }
    public int Year { get; }
}