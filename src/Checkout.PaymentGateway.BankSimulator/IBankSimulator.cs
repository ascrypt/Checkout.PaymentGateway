using Checkout.PaymentGateway.BankSimulator.Models;

namespace Checkout.PaymentGateway.BankSimulator;

public interface IBankSimulator
{
    /// <summary>
    ///     Bank payment process simulation
    /// </summary>
    /// <param name="request"></param>
    /// <returns>bank payment result</returns>
    Task<BankSimulatorResponse> ProcessPaymentAsync(BankSimulatorRequest request);
}