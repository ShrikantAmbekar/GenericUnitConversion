using UnitConversion.Domain.Enums;

namespace UnitConversion.Domain.Entities;

public sealed class UnitDefinition
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public required ConversionCategory Category { get; init; }
    public decimal ConversionFactor { get; init; }
}