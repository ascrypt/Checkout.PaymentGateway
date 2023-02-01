using Checkout.PaymentGateway.Application.Interfaces;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Infrastructure.Repositories.Payments;

public class InMemoryPaymentRepository : IPaymentRepository
{
    private readonly ILogger<InMemoryPaymentRepository> _logger;
    private readonly IMemoryCache _memoryCache;


    public InMemoryPaymentRepository(IMemoryCache memoryCache, ILogger<InMemoryPaymentRepository> logger)
    {
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public async Task<Payment> GetAsync(Guid id)
    {
        var paymentFound = await Task.FromResult(_memoryCache.TryGetValue<Payment>(id, out var payment));
        return paymentFound ? payment : null;
    }

    public async Task UpdateAsync(Payment payment)
    {
        if (await Task.FromResult(!_memoryCache.TryGetValue<Payment>(payment.Id, out var existingPayment)))
            throw new NotFoundPaymentException($"Not found a payment with id {payment.Id}");

        _memoryCache.Set(payment.Id, payment);
    }

    public async Task CreateAsync(Payment payment)
    {
        if (await Task.FromResult(_memoryCache.TryGetValue<Payment>(payment.Id, out var existingPayment)))
        {
            throw new DuplicatePaymentException($"A payment with id {payment.Id} already exists");
        }

        _memoryCache.Set(payment.Id, payment);
    }
}