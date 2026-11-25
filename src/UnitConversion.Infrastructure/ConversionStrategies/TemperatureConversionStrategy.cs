using UnitConversion.Application.Interfaces;
using UnitConversion.Domain.Entities;
using UnitConversion.Domain.Exceptions;

namespace UnitConversion.Infrastructure.ConversionStrategies;

public sealed class TemperatureConversionStrategy : IConversionStrategy
{
    public decimal Convert(decimal value, UnitDefinition fromUnit, UnitDefinition toUnit)
    {
        if (string.Equals(fromUnit.Code, "kelvin", StringComparison.OrdinalIgnoreCase) && value < 0m)
        {
            throw new InvalidConversionValueException("Kelvin value cannot be below absolute zero.");
        }

        var celsius = ToCelsius(value, fromUnit.Code);
        if (celsius < -273.15m)
        {
            throw new InvalidConversionValueException("Temperature is below absolute zero.");
        }

        return FromCelsius(celsius, toUnit.Code);
    }

    private static decimal ToCelsius(decimal value, string unitCode) => unitCode.ToLowerInvariant() switch
    {
        "celsius" => value,
        "fahrenheit" => (value - 32m) * 5m / 9m,
        "kelvin" => value - 273.15m,
        _ => throw new UnitNotFoundException(unitCode)
    };

    private static decimal FromCelsius(decimal celsius, string unitCode) => unitCode.ToLowerInvariant() switch
    {
        "celsius" => celsius,
        "fahrenheit" => celsius * 9m / 5m + 32m,
        "kelvin" => celsius + 273.15m,
        _ => throw new UnitNotFoundException(unitCode)
    };

}