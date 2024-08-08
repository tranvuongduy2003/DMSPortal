using DMSPortal.BackendServer.Authorization;
using DMSPortal.BackendServer.Helpers.HttpResponses;
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
    [Authorize]
    [ClaimRequirement(EFunctionCode.GENERAL_BRANCH, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(List<BranchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBranches()
    {
        var branches = await _branchesService.GetBranchesAsync();

        return Ok(new ApiOkResponse(branches));
    }
    
    [HttpGet("{branchId}/pitches")]
    [Authorize]
    [ClaimRequirement(EFunctionCode.GENERAL_BRANCH, ECommandCode.VIEW)]
    [ClaimRequirement(EFunctionCode.GENERAL_PITCH, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(List<PitchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPitchesByBranchId(string branchId)
    {
        try
        {
            var pitches = await _pitchesService.GetPitchesByBranchIdAsync(branchId);

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
    
    [HttpPost]
    [Authorize]
    [ClaimRequirement(EFunctionCode.GENERAL_BRANCH, ECommandCode.CREATE)]
    [ApiValidationFilter]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBranch([FromBody] CreateBranchRequest request)
    {
        try
        {
            await _branchesService.CreateBranchAsync(request);

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
    
    [HttpPut("{branchId}")]
    [Authorize]
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
    [Authorize]
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

