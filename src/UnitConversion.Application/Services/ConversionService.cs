using Microsoft.Extensions.Logging;
using UnitConversion.Application.DTOs;
using UnitConversion.Application.Interfaces;
using UnitConversion.Domain.Entities;
using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Exceptions;

namespace UnitConversion.Application.Services;

public sealed class ConversionService : IConversionService
{
    private readonly IUnitProvider _unitProvider;
    private readonly IConversionStrategyFactory _strategyFactory;
    private readonly ILogger<ConversionService> _logger;

    public ConversionService(
        IUnitProvider unitProvider,
        IConversionStrategyFactory strategyFactory,
        ILogger<ConversionService> logger)
    {
        _unitProvider = unitProvider;
        _strategyFactory = strategyFactory;
        _logger = logger;
    }

    public ConversionResponse Convert(ConversionRequest request)
    {
        var fromUnit = ResolveUnit(request.FromUnit);
        var toUnit = ResolveUnit(request.ToUnit);

        if (fromUnit.Category != toUnit.Category)
        {
            throw new CrossCategoryConversionException();
        }

        if (string.Equals(fromUnit.Code, toUnit.Code, StringComparison.OrdinalIgnoreCase))
        {
            var sameUnitResponse = BuildResponse(request.Value, fromUnit, toUnit, request.Value);
            LogSuccessfulConversion(sameUnitResponse);
            return sameUnitResponse;
        }

        var strategy = _strategyFactory.GetStrategy(fromUnit.Category);
        var converted = strategy.Convert(request.Value, fromUnit, toUnit);
        var response = BuildResponse(request.Value, fromUnit, toUnit, converted);
        LogSuccessfulConversion(response);
        return response;
    }

    public IReadOnlyList<UnitResponse> GetAllUnits() =>
        _unitProvider.GetAllUnits().Select(MapUnit).ToList();

    public IReadOnlyList<UnitResponse> GetUnitsByCategory(string category)
    {
        if (!Enum.TryParse<ConversionCategory>(category, ignoreCase: true, out var parsed))
        {
            throw new DomainException($"Category '{category}' is not supported.");
        }

        return _unitProvider.GetUnitsByCategory(parsed).Select(MapUnit).ToList();
    }

    private void LogSuccessfulConversion(ConversionResponse response)
    {
        _logger.LogInformation(
            "Conversion succeeded: {OriginalValue} {FromUnit} -> {ConvertedValue} {ToUnit} ({Category})",
            response.OriginalValue,
            response.FromUnit,
            response.ConvertedValue,
            response.ToUnit,
            response.Category);
    }

    private UnitDefinition ResolveUnit(string unitCode)
    {
        var unit = _unitProvider.GetUnit(unitCode);
        if (unit is null)
        {
            throw new UnitNotFoundException(unitCode);
        }

        return unit;
    }

    private static ConversionResponse BuildResponse(
        decimal originalValue,
        UnitDefinition fromUnit,
        UnitDefinition toUnit,
        decimal convertedValue) =>
        new()
        {
            OriginalValue = originalValue,
            FromUnit = fromUnit.Code,
            ToUnit = toUnit.Code,
            ConvertedValue = convertedValue,
            Category = fromUnit.Category.ToString()
        };

    private static UnitResponse MapUnit(UnitDefinition unit) =>
        new()
        {
            Code = unit.Code,
            Name = unit.Name,
            Category = unit.Category.ToString()
        };
}
