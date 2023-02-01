namespace Checkout.PaymentGateway.Application.Common;

public static class CardHelper
{
    public static string MaskCardNumber(string cardNumber, int visibleNumbers)
    {
        return string.Concat(
            new string('*', cardNumber.Length - visibleNumbers),
            cardNumber.AsSpan(cardNumber.Length - visibleNumbers)
            );
    }
}