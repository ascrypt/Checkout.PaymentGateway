using Checkout.PaymentGateway.Domain.Enums;

namespace Checkout.PaymentGateway.Domain.Entities;

public class Payment
{
    public Payment(Guid id, Payer payer, Card card, PaymentResult result, DateTime createDateTime, Money money)
    {
        Id = id;
        Payer = payer;
        Card = card;
        Result = result;
        CreateDateTime = DateTime.Now;
        Money = money;
    }

    public Payment()
    {
    }

    public Guid Id { get; set; }
    public string MerchantId { get; set; }
    public Payer Payer { get; set; }
    public Card Card { get; set; }
    public PaymentResult Result { get; set; }
    public DateTime CreateDateTime { get; set; }
    public Money Money { get; set; }

    public void Approve()
    {
        Result = PaymentResult.Successful;
    }

    public void Decline()
    {
        Result = PaymentResult.Failed;
    }
}