﻿using DMSPortal.BackendServer.Abstractions.Services;
using DMSPortal.BackendServer.Abstractions.UseCases;
using DMSPortal.BackendServer.Attributes;
using DMSPortal.Models.Common;
using DMSPortal.Models.DTOs.Class;
using DMSPortal.Models.DTOs.Student;
using DMSPortal.Models.Enums;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.HttpResponses;
using DMSPortal.Models.Requests.Class;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ClassesController : ControllerBase
{
    private readonly IClassesUseCase _classesUseCase;
    private readonly IStudentsUseCase _studentsUseCase;

    public ClassesController(IClassesUseCase classesUseCase, IStudentsUseCase studentsUseCase)
    {
        _classesUseCase = classesUseCase;
        _studentsUseCase = studentsUseCase;
    }
    
    [HttpGet]
    [ClaimRequirement(EFunctionCode.GENERAL_CLASS, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(Pagination<ClassDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetClasses([FromQuery] PaginationFilter filter)
    {
        var classes = await _classesUseCase.GetClassesAsync(filter);

        return Ok(new ApiOkResponse(classes));
    }
    
    [HttpGet("{classId}/students")]
    [ClaimRequirement(EFunctionCode.GENERAL_CLASS, ECommandCode.VIEW)]
    [ClaimRequirement(EFunctionCode.GENERAL_STUDENT, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(Pagination<StudentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetStudentsByClassId(string classId, [FromQuery] PaginationFilter filter)
    {
        try
        {
            var students = await _studentsUseCase.GetStudentsByClassIdAsync(classId, filter);

            return Ok(new ApiOkResponse(students));
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
    
    [HttpGet("{classId}")]
    [ClaimRequirement(EFunctionCode.GENERAL_STUDENT, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(ClassDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ClassDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetClassById(string classId)
    {
        try
        {
            var classDto = await _classesUseCase.GetClassByIdAsync(classId);

            return Ok(new ApiOkResponse(classDto));
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
    [ClaimRequirement(EFunctionCode.GENERAL_CLASS, ECommandCode.CREATE)]
    [ApiValidationFilter]
    [ProducesResponseType(typeof(ClassDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateClass([FromBody] CreateClassRequest request)
    {
        try
        {
            var classDto = await _classesUseCase.CreateClassAsync(request);

            return Ok(new ApiCreatedResponse(classDto));
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
    
    [HttpPut("{classId}")]
    [ClaimRequirement(EFunctionCode.GENERAL_CLASS, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateClass(string classId, [FromBody] UpdateClassRequest request)
    {
        try
        {
            await _classesUseCase.UpdateClassAsync(classId, request);

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
    
    [HttpDelete("{classId}")]
    [ClaimRequirement(EFunctionCode.GENERAL_CLASS, ECommandCode.DELETE)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteClass(string classId)
    {
        try
        {
            await _classesUseCase.DeleteClassAsync(classId);

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