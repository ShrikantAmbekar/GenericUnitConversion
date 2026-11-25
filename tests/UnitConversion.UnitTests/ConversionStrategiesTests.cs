using UnitConversion.Domain.Entities;
using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Exceptions;
using UnitConversion.Infrastructure.ConversionStrategies;

namespace UnitConversion.UnitTests;

[TestFixture]
public sealed class ConversionStrategiesTests
{
    private static UnitDefinition Unit(string code, ConversionCategory category, decimal factor) =>
        new()
        {
            Code = code,
            Name = code,
            Category = category,
            ConversionFactor = factor
        };

    [Test]
    public void LengthStrategy_MeterToFeet_ReturnsExpected()
    {
        var strategy = new FactorBasedConversionStrategy(ConversionCategory.Length);
        var meter = Unit("meter", ConversionCategory.Length, 1m);
        var feet = Unit("feet", ConversionCategory.Length, 0.3048m);

        var result = strategy.Convert(10m, meter, feet);

        Assert.That(result, Is.EqualTo(32.8084m).Within(0.0001m));
    }

    [Test]
    public void LengthStrategy_KilometerToMeter_ReturnsExpected()
    {
        var strategy = new FactorBasedConversionStrategy(ConversionCategory.Length);
        var km = Unit("kilometer", ConversionCategory.Length, 1000m);
        var meter = Unit("meter", ConversionCategory.Length, 1m);

        var result = strategy.Convert(5m, km, meter);

        Assert.That(result, Is.EqualTo(5000m));
    }

    [Test]
    public void WeightStrategy_KilogramToPound_ReturnsExpected()
    {
        var strategy = new FactorBasedConversionStrategy(ConversionCategory.Weight);
        var kg = Unit("kilogram", ConversionCategory.Weight, 1m);
        var pound = Unit("pound", ConversionCategory.Weight, 0.45359237m);

        var result = strategy.Convert(10m, kg, pound);

        Assert.That(result, Is.EqualTo(22.0462m).Within(0.0001m));
    }

    [Test]
    public void TemperatureStrategy_CelsiusToFahrenheit_ReturnsExpected()
    {
        var strategy = new TemperatureConversionStrategy();
        var celsius = Unit("celsius", ConversionCategory.Temperature, 0m);
        var fahrenheit = Unit("fahrenheit", ConversionCategory.Temperature, 0m);

        var result = strategy.Convert(25m, celsius, fahrenheit);

        Assert.That(result, Is.EqualTo(77m));
    }

    [Test]
    public void TemperatureStrategy_CelsiusToKelvin_ReturnsExpected()
    {
        var strategy = new TemperatureConversionStrategy();
        var celsius = Unit("celsius", ConversionCategory.Temperature, 0m);
        var kelvin = Unit("kelvin", ConversionCategory.Temperature, 0m);

        var result = strategy.Convert(0m, celsius, kelvin);

        Assert.That(result, Is.EqualTo(273.15m));
    }

    [Test]
    public void LengthStrategy_NegativeValue_Throws()
    {
        var strategy = new FactorBasedConversionStrategy(ConversionCategory.Length);
        var meter = Unit("meter", ConversionCategory.Length, 1m);

        Assert.Throws<InvalidConversionValueException>(() =>
            strategy.Convert(-1m, meter, meter));
    }
}