namespace Checkout.PaymentGateway.Application.Interfaces;

/// <summary>
///     Event dispatcher used to publish event
/// </summary>
public interface IPaymentEventHandler
{
    /// <summary>
    ///     Publishes an event
    /// </summary>
    /// <param name="Event">The event</param>
    Task PublishAsync<IEvent>(IEvent Event);
}

public interface IEvent
{
}