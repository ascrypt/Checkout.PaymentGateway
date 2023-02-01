using Checkout.PaymentGateway.Domain.Entities;

namespace Checkout.PaymentGateway.Application.Interfaces;

public interface IPaymentRepository
{
    /// <summary>
    ///     Get a payment
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Payment</returns>
    public Task<Payment> GetAsync(Guid id);

    /// <summary>
    ///     Update a payment
    /// </summary>
    /// <param name="payment">The <see cref="Payment" /></param>
    public Task UpdateAsync(Payment payment);

    /// <summary>
    ///     Create a payment
    /// </summary>
    /// <param name="payment"></param>
    /// <returns></returns>
    public Task CreateAsync(Payment payment);
}