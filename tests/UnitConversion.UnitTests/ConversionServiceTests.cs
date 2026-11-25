using UnitConversion.Application.DTOs;
using UnitConversion.Application.Services;
using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Exceptions;
using UnitConversion.Infrastructure.ConversionStrategies;
using UnitConversion.Infrastructure.Providers;

namespace UnitConversion.UnitTests;

[TestFixture]
public sealed class ConversionServiceTests
{
    private ConversionService _service = null!;

    [SetUp]
    public void SetUp()
    {
        var provider = new InMemoryUnitProvider();
        var factory = new ConversionStrategyFactory();
        _service = new ConversionService(provider, factory);
    }

    [Test]
    public void Convert_SameUnit_ReturnsOriginalValue()
    {
        var result = _service.Convert(new ConversionRequest
        {
            Value = 42m,
            FromUnit = "meter",
            ToUnit = "meter"
        });

        Assert.That(result.ConvertedValue, Is.EqualTo(42m));
        Assert.That(result.Category, Is.EqualTo(ConversionCategory.Length.ToString()));
    }

    [Test]
    public void Convert_CrossCategory_Throws()
    {
        Assert.Throws<CrossCategoryConversionException>(() =>
            _service.Convert(new ConversionRequest
            {
                Value = 10m,
                FromUnit = "meter",
                ToUnit = "kilogram"
            }));
    }

    [Test]
    public void Convert_UnknownUnit_Throws()
    {
        Assert.Throws<UnitNotFoundException>(() =>
            _service.Convert(new ConversionRequest
            {
                Value = 1m,
                FromUnit = "lightyear",
                ToUnit = "meter"
            }));
    }

    [Test]
    public void GetUnitsByCategory_Length_ReturnsUnits()
    {
        var units = _service.GetUnitsByCategory("Length");

        Assert.That(units, Has.Count.EqualTo(5));
        Assert.That(units.All(u => u.Category == "Length"), Is.True);
    }
}