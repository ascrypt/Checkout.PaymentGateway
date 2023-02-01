using AutoMapper;
using Checkout.PaymentGateway.Application.Interfaces;
using Checkout.PaymentGateway.BankSimulator;
using Checkout.PaymentGateway.BankSimulator.Enums;
using Checkout.PaymentGateway.BankSimulator.Models;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.Events;

namespace Checkout.PaymentGateway.Infrastructure.Services;

public class PaymentEventHandler : IPaymentEventHandler
{
    private readonly IBankSimulator _bankSimulator;
    private readonly IMapper _mapper;
    private readonly IPaymentRepository _paymentRepository;

    public PaymentEventHandler(IPaymentRepository paymentRepository, IMapper mapper,
        IBankSimulator bankSimulator)
    {
        _bankSimulator = bankSimulator;
        _mapper = mapper;
        _paymentRepository = paymentRepository;
    }

    public Task PublishAsync<TEvent>(TEvent @event)
    {
        Task.Run(() => MakePayment((@event as PaymentCreated).Payment));

        return Task.CompletedTask;
    }

    private async Task MakePayment(Payment payment)
    {
        var bankRequest = _mapper.Map<BankSimulatorRequest>(payment);
        var response = await _bankSimulator.ProcessPaymentAsync(bankRequest);
        if (response.Result != BankPaymentResult.Approved)
        {
            payment.Decline();
        }
        else
        {
            payment.Approve();
        }

        await _paymentRepository.UpdateAsync(payment);
    }
}