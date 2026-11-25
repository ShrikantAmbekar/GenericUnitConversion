namespace UnitConversion.Application.DTOs;

public sealed class ConversionRequest
{
    public decimal Value { get; init; }
    public required string FromUnit { get; init; }
    public required string ToUnit { get; init; }
}