using DMSPortal.BackendServer.Authorization;
using DMSPortal.BackendServer.Helpers.HttpResponses;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs.Branch;
using DMSPortal.Models.DTOs.PitchGroup;
using DMSPortal.Models.Enums;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Requests.PitchGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PitchGroupsController : ControllerBase
{
    private readonly IPitchGroupsService _pitchGroupsService;
    private readonly IBranchesService _branchesService;

    public PitchGroupsController(IPitchGroupsService pitchGroupsService, IBranchesService branchesService)
    {
        _pitchGroupsService = pitchGroupsService;
        _branchesService = branchesService;
    }
    
    [HttpGet]
    [Authorize]
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH_GROUP, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(List<PitchGroupDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPitchGroups()
    {
        var pitchGroups = await _pitchGroupsService.GetPitchGroupsAsync();

        return Ok(new ApiOkResponse(pitchGroups));
    }
    
    [HttpGet("{pitchGroupId}/branches")]
    [Authorize]
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH_GROUP, ECommandCode.VIEW)]
    [ClaimRequirement(EFunctionCode.GENERAL_BRANCH, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(List<BranchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBranchesByPitchGroupId(string pitchGroupId)
    {
        try
        {
            var branches = await _branchesService.GetBranchesByPitchGroupIdAsync(pitchGroupId);

            return Ok(new ApiOkResponse(branches));
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
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH_GROUP, ECommandCode.CREATE)]
    [ApiValidationFilter]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePitchGroup([FromBody] CreatePitchGroupRequest request)
    {
        try
        {
            await _pitchGroupsService.CreatePitchGroupAsync(request);

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
    
    [HttpPut("{pitchGroupId}")]
    [Authorize]
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH_GROUP, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePitchGroup(string pitchGroupId, [FromBody] UpdatePitchGroupRequest request)
    {
        try
        {
            await _pitchGroupsService.UpdatePitchGroupAsync(pitchGroupId, request);

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
    
    [HttpDelete("{pitchGroupId}")]
    [Authorize]
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH_GROUP, ECommandCode.DELETE)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePitchGroup(string pitchGroupId)
    {
        try
        {
            await _pitchGroupsService.DeletePitchGroupAsync(pitchGroupId);

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
