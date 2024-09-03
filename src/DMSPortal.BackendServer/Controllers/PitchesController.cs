using DMSPortal.BackendServer.Attributes;
using DMSPortal.BackendServer.Helpers.HttpResponses;
using DMSPortal.BackendServer.Models;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs.Class;
using DMSPortal.Models.DTOs.Pitch;
using DMSPortal.Models.Enums;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Requests.Pitch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PitchesController : ControllerBase
{
    private readonly IPitchesService _pitchesService;
    private readonly IClassesService _classesService;

    public PitchesController(IPitchesService pitchesService, IClassesService classesService)
    {
        _pitchesService = pitchesService;
        _classesService = classesService;
    }

    [HttpGet]
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(Pagination<PitchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPitches([FromQuery] PaginationFilter filter)
    {
        var pitches = await _pitchesService.GetPitchesAsync(filter);

        return Ok(new ApiOkResponse(pitches));
    }

    [HttpGet("{pitchId}/classes")]
    [ClaimRequirement(EFunctionCode.GENERAL_CLASS, ECommandCode.VIEW)]
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(Pagination<ClassDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetClassesByPitchId(string pitchId, [FromQuery] PaginationFilter filter)
    {
        try
        {
            var classes = await _classesService.GetClassesByPitchIdAsync(pitchId, filter);

            return Ok(new ApiOkResponse(classes));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("{pitchId}")]
    [Authorize]
    [ClaimRequirement(EFunctionCode.GENERAL_STUDENT, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(PitchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PitchDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPitchById(string pitchId)
    {
        try
        {
            var pitch = await _pitchesService.GetPitchByIdAsync(pitchId);

            return Ok(new ApiOkResponse(pitch));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost]
    [Authorize]
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH, ECommandCode.CREATE)]
    [ApiValidationFilter]
    [ProducesResponseType(typeof(PitchDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePitch([FromBody] CreatePitchRequest request)
    {
        try
        {
            var pitch = await _pitchesService.CreatePitchAsync(request);

            return Ok(new ApiCreatedResponse(pitch));
        }
        catch (BadRequestException e)
        {
            return BadRequest(new ApiBadRequestResponse(e.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPut("{pitchId}")]
    [Authorize]
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePitch(string pitchId, [FromBody] UpdatePitchRequest request)
    {
        try
        {
            await _pitchesService.UpdatePitchAsync(pitchId, request);

            return Ok(new ApiOkResponse());
        }
        catch (BadRequestException e)
        {
            return BadRequest(new ApiBadRequestResponse(e.Message));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpDelete("{pitchId}")]
    [Authorize]
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH, ECommandCode.DELETE)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePitch(string pitchId)
    {
        try
        {
            await _pitchesService.DeletePitchAsync(pitchId);

            return Ok(new ApiOkResponse());
        }
        catch (BadRequestException e)
        {
            return BadRequest(new ApiBadRequestResponse(e.Message));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }
}