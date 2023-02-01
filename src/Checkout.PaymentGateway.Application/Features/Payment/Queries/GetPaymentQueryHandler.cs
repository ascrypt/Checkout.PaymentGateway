using AutoMapper;
using Checkout.PaymentGateway.Application.Common;
using Checkout.PaymentGateway.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Application.Features.Payment.Queries;

public class GetPaymentQueryHandler : IRequestHandler<GetPaymentQuery, GetPaymentQueryResult>
{
    private readonly ILogger<GetPaymentQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IPaymentRepository _paymentRepository;

    public GetPaymentQueryHandler(ILogger<GetPaymentQueryHandler> logger, IPaymentRepository paymentRepository,
        IMapper mapper)
    {
        _logger = logger;
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task<GetPaymentQueryResult> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetAsync(request.Id); 
        _logger.LogInformation($"Payment id {request.Id} requested.");

        if (payment is null)
            return null;
        
        var paymentResponse = _mapper.Map<GetPaymentQueryResult>(payment);
        paymentResponse.CardNumber = CardHelper.MaskCardNumber(payment.Card.Number, 4);

        return paymentResponse;
    }
}