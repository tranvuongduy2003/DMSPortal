﻿using DMSPortal.BackendServer.Attributes;
using DMSPortal.BackendServer.Helpers.HttpResponses;
using DMSPortal.BackendServer.Models;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs.User;
using DMSPortal.Models.Enums;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Requests.User;
using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }
    
    [HttpGet]
    [ClaimRequirement(EFunctionCode.SYSTEM_USER, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUsers([FromQuery] PaginationFilter filter)
    {
        var users = await _usersService.GetUsersAsync(filter);

        return Ok(new ApiOkResponse(users));
    }
    
    [HttpGet("{userId}")]
    [ClaimRequirement(EFunctionCode.SYSTEM_USER, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(string userId)
    {
        try
        {
            var user = await _usersService.GetUserByIdAsync(userId);

            return Ok(new ApiOkResponse(user));
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
    [ClaimRequirement(EFunctionCode.SYSTEM_USER, ECommandCode.CREATE)]
    [ApiValidationFilter]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var user = await _usersService.CreateUserAsync(request);

            return Ok(new ApiCreatedResponse(user));
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
    
    [HttpPut("{userId}")]
    [ClaimRequirement(EFunctionCode.SYSTEM_USER, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserRequest request)
    {
        try
        {
            await _usersService.UpdateUserAsync(userId, request);

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
    
    [HttpDelete("{userId}")]
    [ClaimRequirement(EFunctionCode.SYSTEM_USER, ECommandCode.DELETE)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        try
        {
            await _usersService.DeleteUserAsync(userId);

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