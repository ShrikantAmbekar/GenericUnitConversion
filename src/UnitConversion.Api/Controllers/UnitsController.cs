using Microsoft.AspNetCore.Mvc;
using UnitConversion.Application.DTOs;
using UnitConversion.Api.Models;
using UnitConversion.Application.Interfaces;

namespace UnitConversion.Api.Controllers;

[ApiController]
[Route("api/v1/units")]
public sealed class UnitsController : ControllerBase
{
    private readonly IConversionService _conversionService;

    public UnitsController(IConversionService conversionService)
    {
        _conversionService = conversionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UnitResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(_conversionService.GetAllUnits());
    }

    [HttpGet("{category}")]
    [ProducesResponseType(typeof(IReadOnlyList<UnitResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult GetByCategory(string category)
    {
        var units = _conversionService.GetUnitsByCategory(category);
        return Ok(units);
    }
}