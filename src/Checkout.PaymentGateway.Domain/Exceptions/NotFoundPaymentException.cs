namespace Checkout.PaymentGateway.Domain.Exceptions
{
    public class NotFoundPaymentException : Exception
    {
        public NotFoundPaymentException()
        {
        }

        public NotFoundPaymentException(string message)
            : base(message)
        {
        }

        public NotFoundPaymentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
