using AutoMapper;
using Checkout.PaymentGateway.Application.Interfaces;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.Enums;
using Checkout.PaymentGateway.Domain.Events;
using Checkout.PaymentGateway.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Application.Features.Payment.Commands;

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, CreatePaymentCommandResult>
{
    private readonly IPaymentEventHandler _handler;
    private readonly ILogger<CreatePaymentCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IPaymentRepository _paymentRepository;

    public CreatePaymentCommandHandler(IPaymentRepository paymentRepository,
        ILogger<CreatePaymentCommandHandler> logger, IMapper mapper, IPaymentEventHandler paymentEventHandler)
    {
        _logger = logger;
        _paymentRepository = paymentRepository;
        _mapper = mapper;
        _handler = paymentEventHandler;
    }

    public async Task<CreatePaymentCommandResult> Handle(CreatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var paymentResponse = new CreatePaymentCommandResult();

        try
        {
            var expirationDate = new CardExpirationDate(request.ExpirationMonth, request.ExpirationYear);
            var card = new Card(request.CardHolderName, request.CvvCode, request.CardNumber, expirationDate);

            var payment = new Domain.Entities.Payment
            {
                Card = card,
                CreateDateTime = DateTime.Now,
                Id = request.Id,
                MerchantId = request.MerchantId,
                Money = new Money(request.Amount, request.Currency)
            };
            await _paymentRepository.CreateAsync(payment);
            paymentResponse = _mapper.Map<CreatePaymentCommandResult>(payment);

            await _handler.PublishAsync(new PaymentCreated(payment));

            _logger.LogInformation(
                $"Payment processing started by card number ending with {request.CardNumber.Substring(12, 4)}");
        }

        catch (DuplicatePaymentException ex)
        {
            paymentResponse.ResultCode = PaymentResult.Duplicate;

            _logger.LogError(
                $"Duplicate payment failed for card number ending with {request.CardNumber.Substring(12, 4)}. {ex.Message}");
        }
        catch (Exception ex)
        {
            paymentResponse.ResultCode = PaymentResult.Failed;

            _logger.LogError(
                $"Payment failed for card number ending with {request.CardNumber.Substring(12, 4)}. {ex.Message}");
        }

        return paymentResponse;
    }
}