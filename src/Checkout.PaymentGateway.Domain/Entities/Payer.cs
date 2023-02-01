namespace Checkout.PaymentGateway.Domain.Entities;

public class Payer
{
    public Payer(PayerAddress address, Card card)
    {
        Address = address;
        Card = card;
    }

    public PayerAddress Address { get; set; }
    public Card Card { get; set; }
}