namespace UnitConversion.Domain.Exceptions;

public sealed class CrossCategoryConversionException : DomainException
{
    public CrossCategoryConversionException()
        : base("Conversion between different categories is not supported.")
    {
    }
}