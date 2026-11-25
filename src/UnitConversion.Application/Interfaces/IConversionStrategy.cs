using UnitConversion.Domain.Entities;

namespace UnitConversion.Application.Interfaces;

public interface IConversionStrategy
{
    decimal Convert(decimal value, UnitDefinition fromUnit, UnitDefinition toUnit);
}