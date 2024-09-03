using DMSPortal.BackendServer.Attributes;
using DMSPortal.BackendServer.Helpers.HttpResponses;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs.Permission;
using DMSPortal.Models.Enums;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionsService _permissionsService;

    public PermissionsController(IPermissionsService permissionsService)
    {
        _permissionsService = permissionsService;
    }

    [HttpGet]
    [ClaimRequirement(EFunctionCode.SYSTEM_PERMISSION, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(List<PermissionScreenDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCommandViews()
    {
        var permissions = await _permissionsService.GetCommandViewsAsync();

        return Ok(new ApiOkResponse(permissions));
    }

    [HttpGet("roles")]
    [ClaimRequirement(EFunctionCode.SYSTEM_PERMISSION, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(List<RolePermissionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRolePermissions()
    {
        var roles = await _permissionsService.GetRolePermissionsAsync();

        return Ok(new ApiOkResponse(roles));
    }

    [HttpPut]
    [ClaimRequirement(EFunctionCode.SYSTEM_PERMISSION, ECommandCode.UPDATE)]
    [ProducesResponseType(typeof(List<PermissionScreenDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutPermissionByCommand([FromBody] UpdatePermissionByCommandRequest request)
    {
        try
        {
            await _permissionsService.UpdatePermissionByCommand(request);

            return await GetCommandViews();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiNotFoundResponse(ex.Message));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ApiBadRequestResponse(ex.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPut("roles")]
    [ClaimRequirement(EFunctionCode.SYSTEM_PERMISSION, ECommandCode.UPDATE)]
    [ProducesResponseType(typeof(List<RolePermissionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutPermissionByRole([FromBody] UpdatePermissionByRoleRequest request)
    {
        try
        {
            await _permissionsService.UpdatePermissionByRole(request);

            return await GetRolePermissions();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiNotFoundResponse(ex.Message));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ApiBadRequestResponse(ex.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }
}