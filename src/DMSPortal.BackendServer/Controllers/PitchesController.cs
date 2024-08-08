using DMSPortal.BackendServer.Authorization;
using DMSPortal.BackendServer.Helpers.HttpResponses;
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
    [Authorize]
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(List<PitchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPitches()
    {
        var pitches = await _pitchesService.GetPitchesAsync();

        return Ok(new ApiOkResponse(pitches));
    }
    
    [HttpGet("{pitchId}/classes")]
    [Authorize]
    [ClaimRequirement(EFunctionCode.GENERAL_CLASS, ECommandCode.VIEW)]
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(List<ClassDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetClassesByPitchId(string pitchId)
    {
        try
        {
            var classes = await _classesService.GetClassesByPitchIdAsync(pitchId);

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
    
    [HttpPost]
    [Authorize]
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH, ECommandCode.CREATE)]
    [ApiValidationFilter]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePitch([FromBody] CreatePitchRequest request)
    {
        try
        {
            await _pitchesService.CreatePitchAsync(request);

            return Ok(new ApiCreatedResponse());
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