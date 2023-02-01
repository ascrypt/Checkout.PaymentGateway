using Checkout.PaymentGateway.BankSimulator.Enums;
using Checkout.PaymentGateway.BankSimulator.Models;

namespace Checkout.PaymentGateway.BankSimulator;

public class BankSimulator : IBankSimulator
{
    /// <summary>
    ///     Bank payment process simulation
    /// </summary>
    /// <param name="request"></param>
    /// <returns>bank payment result</returns>
    public async Task<BankSimulatorResponse> ProcessPaymentAsync(BankSimulatorRequest request)
    {
        var randomNumber = new Random();
        Thread.Sleep(randomNumber.Next(10000, 15000));
        var statusValues = Enum.GetValues(typeof(BankPaymentResult));
        var randomPaymentResult = (BankPaymentResult)(statusValues.GetValue(randomNumber.Next(0,2)));

        return await Task.FromResult(new BankSimulatorResponse(request.PaymentId, randomPaymentResult));
    }
}