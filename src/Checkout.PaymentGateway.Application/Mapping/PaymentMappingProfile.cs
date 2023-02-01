using AutoMapper;
using Checkout.PaymentGateway.Application.Features.Payment.Commands;
using Checkout.PaymentGateway.Application.Features.Payment.Queries;
using Checkout.PaymentGateway.BankSimulator.Models;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.Enums;

namespace Checkout.PaymentGateway.Application.Mapping;

public class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        
        CreateMap<Payment, GetPaymentQueryResult>()
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Money.Currency))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Money.Amount))
            .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.Card.Number))
            .ForMember(dest => dest.ExpirationMonth, opt => opt.MapFrom(src => src.Card.ExpirationDate.Month))
            .ForMember(dest => dest.ExpirationYear, opt => opt.MapFrom(src => src.Card.ExpirationDate.Year));
        
        CreateMap<Payment, CreatePaymentCommandResult>()
            .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ResultCode, opt => opt.MapFrom(src => PaymentResult.Processing));

        CreateMap<Payment, BankSimulatorRequest>()
            .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.Card.Number))
            .ForMember(dest => dest.Cvv, opt => opt.MapFrom(src => src.Card.CvvCode))
            .ForMember(dest => dest.ExpirationYear, opt => opt.MapFrom(src => src.Card.ExpirationDate.Year))
            .ForMember(dest => dest.ExpirationMonth, opt => opt.MapFrom(src => src.Card.ExpirationDate.Month))
            .ForMember(dest => dest.CardHolderName, opt => opt.MapFrom(src => src.Card.CardHolderName))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Money.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Money.Currency));
    }
}