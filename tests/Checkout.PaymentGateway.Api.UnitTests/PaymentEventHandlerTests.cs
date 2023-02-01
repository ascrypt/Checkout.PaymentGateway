using AutoMapper;
using Checkout.PaymentGateway.Application.Interfaces;
using Checkout.PaymentGateway.BankSimulator;
using Checkout.PaymentGateway.BankSimulator.Enums;
using Checkout.PaymentGateway.BankSimulator.Models;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.Enums;
using Checkout.PaymentGateway.Domain.Events;
using Checkout.PaymentGateway.Infrastructure.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Checkout.PaymentGateway.Api.UnitTests;

public class PaymentEventHandlerTests
{
    private readonly Mock<IBankSimulator> _bankSimulator = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IPaymentRepository> _repositoryMock = new();

    private readonly IPaymentEventHandler _paymentEventHandler;

    public PaymentEventHandlerTests()
    {
        _paymentEventHandler = new PaymentEventHandler(_repositoryMock.Object, _mapperMock.Object, _bankSimulator.Object);
    }

    [Fact]
    public void Publish_Should_Dispatch_Event_If_Valid()
    {
        // Arrange
        const string holderName = "John Doe";
        const string cardNumber = "7592140428574371";
        const string cvv = "265";
        
        var expirationDate = new CardExpirationDate(10, 29);
        var card = new Card(holderName, cvv, cardNumber, expirationDate);
        var money = new Money(50, "USD");
        var paymentId = Guid.Parse("d3058b2e-2149-4f46-a8a2-b52924f49bde");
        var payer = new Payer(new PayerAddress("23411", "334  Amsterdam"), card);
        var payment = new Payment(paymentId, payer, card, PaymentResult.Processing, DateTime.Now, money);

        var bankRequest = new BankSimulatorRequest(
            paymentId,
            holderName,
            card.Number,
            expirationDate.Year,
            expirationDate.Month,
            money.Amount,
            card.CvvCode,
            money.Currency);

        var bankResponse = new BankSimulatorResponse(paymentId, BankPaymentResult.Approved);

        _bankSimulator.Setup(e => e.ProcessPaymentAsync(bankRequest)).ReturnsAsync(bankResponse);

        // Act
        Action action = () => _paymentEventHandler.PublishAsync(new PaymentCreated(payment));

        // Assert
        action.Should().NotThrow<Exception>();
    }
}