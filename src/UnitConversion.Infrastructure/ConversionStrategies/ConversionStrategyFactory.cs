using UnitConversion.Application.Interfaces;
using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Exceptions;

namespace UnitConversion.Infrastructure.ConversionStrategies;

public sealed class ConversionStrategyFactory : IConversionStrategyFactory
{
    private readonly IReadOnlyDictionary<ConversionCategory, IConversionStrategy> _strategies;

    public ConversionStrategyFactory()
    {
        _strategies = new Dictionary<ConversionCategory, IConversionStrategy>
        {
            [ConversionCategory.Length] = new FactorBasedConversionStrategy(ConversionCategory.Length),
            [ConversionCategory.Weight] = new FactorBasedConversionStrategy(ConversionCategory.Weight),
            [ConversionCategory.Temperature] = new TemperatureConversionStrategy()
        };
    }

    public IConversionStrategy GetStrategy(ConversionCategory category)
    {
        if (!_strategies.TryGetValue(category, out var strategy))
        {
            throw new DomainException($"No conversion strategy registered for category '{category}'.");
        }

        return strategy;
    }
}