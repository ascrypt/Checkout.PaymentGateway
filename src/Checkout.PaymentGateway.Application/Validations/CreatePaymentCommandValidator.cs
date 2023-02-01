using Checkout.PaymentGateway.Application.Features.Payment.Commands;
using FluentValidation;

namespace Checkout.PaymentGateway.Application.Validations;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(v => v.Amount)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(v => v.Currency)
            .MaximumLength(3)
            .MinimumLength(3)
            .NotEmpty();

        RuleFor(v => v.CardNumber)
            .CreditCard()
            .NotEmpty();

        RuleFor(v => v.ExpirationMonth)
            .NotEmpty()
            .GreaterThan(0)
            .LessThan(13);

        RuleFor(v => v.ExpirationYear)
            .NotEmpty()
            .GreaterThanOrEqualTo(Convert.ToInt16(DateTime.Now.Year.ToString().Substring(2, 2)));

        RuleFor(v => v.CvvCode)
            .NotEmpty()
            .MaximumLength(3)
            .MinimumLength(3);

        RuleFor(v => v.CardHolderName)
            .NotEmpty()
            .MinimumLength(5);
        
        RuleFor(v => v.MerchantId)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(50);
    }
}