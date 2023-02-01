namespace Checkout.PaymentGateway.Domain.Entities;

public class Card
{
    public Card(string cardHolderName, string cvvCode, string number, CardExpirationDate expirationDate)
    {
        CardHolderName = cardHolderName;
        CvvCode = cvvCode;
        Number = number;
        ExpirationDate = expirationDate;
    }

    public string CardHolderName { get; set; }
    public string CvvCode { get; set; }
    public string Number { get; set; }
    public CardExpirationDate ExpirationDate { get; set; }
}