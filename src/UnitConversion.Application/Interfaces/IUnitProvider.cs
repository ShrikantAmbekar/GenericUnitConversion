using UnitConversion.Domain.Entities;
using UnitConversion.Domain.Enums;

namespace UnitConversion.Application.Interfaces;

public interface IUnitProvider
{
    UnitDefinition? GetUnit(string unitCode);
    IEnumerable<UnitDefinition> GetAllUnits();
    IEnumerable<UnitDefinition> GetUnitsByCategory(ConversionCategory category);
}