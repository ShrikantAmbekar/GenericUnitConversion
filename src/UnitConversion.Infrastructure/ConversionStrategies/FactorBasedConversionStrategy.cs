using UnitConversion.Application.Interfaces;
using UnitConversion.Domain.Entities;
using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Exceptions;

namespace UnitConversion.Infrastructure.ConversionStrategies;

public sealed class FactorBasedConversionStrategy : IConversionStrategy
{
    private readonly ConversionCategory _category;

    public FactorBasedConversionStrategy(ConversionCategory category)
    {
        _category = category;
    }

    public decimal Convert(decimal value, UnitDefinition fromUnit, UnitDefinition toUnit)
    {
        ValidateValue(value, toUnit);

        var valueInBase = value * fromUnit.ConversionFactor;
        return valueInBase / toUnit.ConversionFactor;
    }

    private void ValidateValue(decimal value, UnitDefinition toUnit)
    {
        if (_category is ConversionCategory.Length or ConversionCategory.Weight && value < 0)
        {
            throw new InvalidConversionValueException(
                $"Negative values are not valid for {_category.ToString().ToLowerInvariant()} conversions.");
        }
    }
}