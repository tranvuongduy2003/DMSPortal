using DMSPortal.BackendServer.Attributes;
using DMSPortal.BackendServer.Helpers.HttpResponses;
using DMSPortal.BackendServer.Models;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs.Branch;
using DMSPortal.Models.DTOs.Pitch;
using DMSPortal.Models.Enums;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Requests.Branch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BranchesController : ControllerBase
{
    private readonly IBranchesService _branchesService;
    private readonly IPitchesService _pitchesService;

    public BranchesController(IBranchesService branchesService, IPitchesService pitchesService)
    {
        _branchesService = branchesService;
        _pitchesService = pitchesService;
    }
    
    [HttpGet]
    [ClaimRequirement(EFunctionCode.GENERAL_BRANCH, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(Pagination<BranchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBranches([FromQuery] PaginationFilter filter)
    {
        var branches = await _branchesService.GetBranchesAsync(filter);

        return Ok(new ApiOkResponse(branches));
    }
    
    [HttpGet("{branchId}/pitches")]
    [ClaimRequirement(EFunctionCode.GENERAL_BRANCH, ECommandCode.VIEW)]
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(Pagination<PitchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPitchesByBranchId(string branchId, [FromQuery] PaginationFilter filter)
    {
        try
        {
            var pitches = await _pitchesService.GetPitchesByBranchIdAsync(branchId, filter);

            return Ok(new ApiOkResponse(pitches));
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
    
    [HttpGet("{branchId}")]
    [ClaimRequirement(EFunctionCode.GENERAL_STUDENT, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(BranchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BranchDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBranchById(string branchId)
    {
        try
        {
            var branch = await _branchesService.GetBranchByIdAsync(branchId);

            return Ok(new ApiOkResponse(branch));
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
    [ClaimRequirement(EFunctionCode.GENERAL_BRANCH, ECommandCode.CREATE)]
    [ApiValidationFilter]
    [ProducesResponseType(typeof(BranchDto),StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBranch([FromBody] CreateBranchRequest request)
    {
        try
        {
            var branch = await _branchesService.CreateBranchAsync(request);

            return Ok(new ApiCreatedResponse(branch));
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
    
    [HttpPut("{branchId}")]
    [ClaimRequirement(EFunctionCode.GENERAL_BRANCH, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBranch(string branchId, [FromBody] UpdateBranchRequest request)
    {
        try
        {
            await _branchesService.UpdateBranchAsync(branchId, request);

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
    
    [HttpDelete("{branchId}")]
    [ClaimRequirement(EFunctionCode.GENERAL_BRANCH, ECommandCode.DELETE)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBranch(string branchId)
    {
        try
        {
            await _branchesService.DeleteBranchAsync(branchId);

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

