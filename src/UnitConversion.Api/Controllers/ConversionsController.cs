using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UnitConversion.Application.DTOs;
using UnitConversion.Api.Models;
using UnitConversion.Application.Interfaces;

namespace UnitConversion.Api.Controllers;

[ApiController]
[Route("api/v1/conversions")]
public sealed class ConversionsController : ControllerBase
{
    private readonly IConversionService _conversionService;
    private readonly IValidator<ConversionRequest> _validator;

    public ConversionsController(
        IConversionService conversionService,
        IValidator<ConversionRequest> validator)
    {
        _conversionService = conversionService;
        _validator = validator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ConversionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Convert([FromBody] ConversionRequest request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var message = string.Join(" ", validation.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, message));
        }

        var result = _conversionService.Convert(request);
        return Ok(result);
    }
}