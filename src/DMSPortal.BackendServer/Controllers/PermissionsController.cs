using DMSPortal.BackendServer.Authorization;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Helpers.HttpResponses;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs;
using DMSPortal.Models.Enums;
using DMSPortal.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Controllers;

[Route("api/permissions")]
[ApiController]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionsService _permissionsService;

    public PermissionsController(IPermissionsService permissionsService)
    {
        _permissionsService = permissionsService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<PermissionScreenDto>), StatusCodes.Status200OK)]
    [ClaimRequirement(EFunctionCode.SYSTEM_PERMISSION, ECommandCode.VIEW)]
    public async Task<IActionResult> GetCommandViews()
    {
        var permissions = await _permissionsService.GetCommandViewsAsync();

        return Ok(new ApiOkResponse(permissions));
    }

    [HttpGet("roles")]
    [ProducesResponseType(typeof(List<RolePermissionDto>), StatusCodes.Status200OK)]
    [ClaimRequirement(EFunctionCode.SYSTEM_PERMISSION, ECommandCode.VIEW)]
    public async Task<IActionResult> GetRolePermissions()
    {
        var roles = await _permissionsService.GetRolePermissionsAsync();
        
        return Ok(new ApiOkResponse(roles));
    }

    [HttpPut]
    [ProducesResponseType(typeof(List<PermissionScreenDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ClaimRequirement(EFunctionCode.SYSTEM_PERMISSION, ECommandCode.UPDATE)]
    public async Task<IActionResult> PutPermissionByCommand([FromBody] UpdatePermissionByCommandRequest request)
    {
        try
        {
            await _permissionsService.UpdatePermissionByCommand(request);

            return await GetCommandViews();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiNotFoundResponse(ex.Message));
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(new ApiBadRequestResponse(ex.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    [HttpPut("roles")]
    [ProducesResponseType(typeof(List<RolePermissionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ClaimRequirement(EFunctionCode.SYSTEM_PERMISSION, ECommandCode.UPDATE)]
    public async Task<IActionResult> PutPermissionByRole([FromBody] UpdatePermissionByRoleRequest request)
    {
        try
        {
            await _permissionsService.UpdatePermissionByRole(request);

            return await GetRolePermissions();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiNotFoundResponse(ex.Message));
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(new ApiBadRequestResponse(ex.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }
}