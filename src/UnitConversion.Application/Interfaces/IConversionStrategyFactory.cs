using UnitConversion.Domain.Enums;

namespace UnitConversion.Application.Interfaces;

public interface IConversionStrategyFactory
{
    IConversionStrategy GetStrategy(ConversionCategory category);
}