namespace Checkout.PaymentGateway.Domain.Entities;

public class PayerAddress
{
    public PayerAddress(string zipCode, string adressLine)
    {
        ZipCode = zipCode;
        AdressLine = adressLine;
    }

    public string ZipCode { get; set; }
    public string AdressLine { get; set; }
}