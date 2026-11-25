using UnitConversion.Application.Interfaces;
using UnitConversion.Domain.Entities;
using UnitConversion.Domain.Enums;

namespace UnitConversion.Infrastructure.Providers;

public sealed class InMemoryUnitProvider : IUnitProvider
{
    private readonly IReadOnlyDictionary<string, UnitDefinition> _units;

    public InMemoryUnitProvider()
    {
        _units = CreateCatalog()
            .ToDictionary(u => u.Code, StringComparer.OrdinalIgnoreCase);
    }

    public UnitDefinition? GetUnit(string unitCode) =>
        _units.TryGetValue(unitCode, out var unit) ? unit : null;

    public IEnumerable<UnitDefinition> GetAllUnits() => _units.Values;

    public IEnumerable<UnitDefinition> GetUnitsByCategory(ConversionCategory category) =>
        _units.Values.Where(u => u.Category == category);

    private static IEnumerable<UnitDefinition> CreateCatalog() =>
    [
        new UnitDefinition { Code = "meter", Name = "Meter", Category = ConversionCategory.Length, ConversionFactor = 1m },
        new UnitDefinition { Code = "kilometer", Name = "Kilometer", Category = ConversionCategory.Length, ConversionFactor = 1000m },
        new UnitDefinition { Code = "feet", Name = "Feet", Category = ConversionCategory.Length, ConversionFactor = 0.3048m },
        new UnitDefinition { Code = "inch", Name = "Inch", Category = ConversionCategory.Length, ConversionFactor = 0.0254m },
        new UnitDefinition { Code = "mile", Name = "Mile", Category = ConversionCategory.Length, ConversionFactor = 1609.344m },

        new UnitDefinition { Code = "kilogram", Name = "Kilogram", Category = ConversionCategory.Weight, ConversionFactor = 1m },
        new UnitDefinition { Code = "gram", Name = "Gram", Category = ConversionCategory.Weight, ConversionFactor = 0.001m },
        new UnitDefinition { Code = "pound", Name = "Pound", Category = ConversionCategory.Weight, ConversionFactor = 0.45359237m },
        new UnitDefinition { Code = "ounce", Name = "Ounce", Category = ConversionCategory.Weight, ConversionFactor = 0.028349523125m },

        new UnitDefinition { Code = "celsius", Name = "Celsius", Category = ConversionCategory.Temperature, ConversionFactor = 0m },
        new UnitDefinition { Code = "fahrenheit", Name = "Fahrenheit", Category = ConversionCategory.Temperature, ConversionFactor = 0m },
        new UnitDefinition { Code = "kelvin", Name = "Kelvin", Category = ConversionCategory.Temperature, ConversionFactor = 0m }
    ];
}