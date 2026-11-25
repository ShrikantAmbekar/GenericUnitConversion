namespace UnitConversion.Domain.Exceptions;

public sealed class InvalidConversionValueException : DomainException
{
    public InvalidConversionValueException(string message) : base(message)
    {
    }
}