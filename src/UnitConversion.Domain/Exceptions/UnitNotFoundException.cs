namespace UnitConversion.Domain.Exceptions;

public sealed class UnitNotFoundException : DomainException
{
    public UnitNotFoundException(string unitCode)
        : base($"Unit '{unitCode}' was not found.")
    {
        UnitCode = unitCode;
    }

    public string UnitCode { get; }
}