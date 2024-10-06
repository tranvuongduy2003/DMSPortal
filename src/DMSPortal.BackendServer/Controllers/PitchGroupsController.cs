using DMSPortal.BackendServer.Abstractions.Services;
using DMSPortal.BackendServer.Abstractions.UseCases;
using DMSPortal.BackendServer.Attributes;
using DMSPortal.Models.Common;
using DMSPortal.Models.DTOs.Branch;
using DMSPortal.Models.DTOs.PitchGroup;
using DMSPortal.Models.Enums;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.HttpResponses;
using DMSPortal.Models.Requests.PitchGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PitchGroupsController : ControllerBase
{
    private readonly IPitchGroupsUseCase _pitchGroupsUseCase;
    private readonly IBranchesUseCase _branchesUseCase;

    public PitchGroupsController(IPitchGroupsUseCase pitchGroupsUseCase, IBranchesUseCase branchesUseCase)
    {
        _pitchGroupsUseCase = pitchGroupsUseCase;
        _branchesUseCase = branchesUseCase;
    }
    
    [HttpGet]
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH_GROUP, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(Pagination<PitchGroupDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPitchGroups([FromQuery] PaginationFilter filter)
    {
        var pitchGroups = await _pitchGroupsUseCase.GetPitchGroupsAsync(filter);

        return Ok(new ApiOkResponse(pitchGroups));
    }
    
    [HttpGet("{pitchGroupId}/branches")]
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH_GROUP, ECommandCode.VIEW)]
    [ClaimRequirement(EFunctionCode.GENERAL_BRANCH, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(Pagination<BranchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBranchesByPitchGroupId(string pitchGroupId, [FromQuery] PaginationFilter filter)
    {
        try
        {
            var branches = await _branchesUseCase.GetBranchesByPitchGroupIdAsync(pitchGroupId, filter);

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
    
    [HttpGet("{pitchGroupId}")]
    [ClaimRequirement(EFunctionCode.GENERAL_STUDENT, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(PitchGroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PitchGroupDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPitchGroupById(string pitchGroupId)
    {
        try
        {
            var pitchGroup = await _pitchGroupsUseCase.GetPitchGroupByIdAsync(pitchGroupId);

            return Ok(new ApiOkResponse(pitchGroup));
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
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH_GROUP, ECommandCode.CREATE)]
    [ApiValidationFilter]
    [ProducesResponseType(typeof(PitchGroupDto),StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePitchGroup([FromBody] CreatePitchGroupRequest request)
    {
        try
        {
            var pitchGroupDto = await _pitchGroupsUseCase.CreatePitchGroupAsync(request);

            return Ok(new ApiCreatedResponse(pitchGroupDto));
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
            await _pitchGroupsUseCase.UpdatePitchGroupAsync(pitchGroupId, request);

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
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH_GROUP, ECommandCode.DELETE)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePitchGroup(string pitchGroupId)
    {
        try
        {
            await _pitchGroupsUseCase.DeletePitchGroupAsync(pitchGroupId);

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
