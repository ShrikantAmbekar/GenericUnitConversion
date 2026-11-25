namespace UnitConversion.Application.DTOs;

public sealed class ConversionResponse
{
    public decimal OriginalValue { get; init; }
    public required string FromUnit { get; init; }
    public required string ToUnit { get; init; }
    public decimal ConvertedValue { get; init; }
    public required string Category { get; init; }
}