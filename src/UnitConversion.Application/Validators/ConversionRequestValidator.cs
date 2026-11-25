using FluentValidation;
using UnitConversion.Application.DTOs;

namespace UnitConversion.Application.Validators;

public sealed class ConversionRequestValidator : AbstractValidator<ConversionRequest>
{
    public ConversionRequestValidator()
    {
        RuleFor(x => x.FromUnit)
            .NotEmpty()
            .WithMessage("fromUnit is required.");

        RuleFor(x => x.ToUnit)
            .NotEmpty()
            .WithMessage("toUnit is required.");
    }
}