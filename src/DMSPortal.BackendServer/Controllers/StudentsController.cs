﻿using DMSPortal.BackendServer.Attributes;
using DMSPortal.BackendServer.Helpers.HttpResponses;
using DMSPortal.BackendServer.Models;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs.Student;
using DMSPortal.Models.Enums;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Requests.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IStudentsService _studentsService;

    public StudentsController(IStudentsService studentsService)
    {
        _studentsService = studentsService;
    }
    
    [HttpGet]
    [ClaimRequirement(EFunctionCode.GENERAL_STUDENT, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(List<StudentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetStudents([FromQuery] PaginationFilter filter)
    {
        var students = await _studentsService.GetStudentsAsync(filter);

        return Ok(new ApiOkResponse(students));
    }
    
    [HttpGet("{studentId}")]
    [ClaimRequirement(EFunctionCode.GENERAL_STUDENT, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetStudentById(string studentId)
    {
        try
        {
            var student = await _studentsService.GetStudentByIdAsync(studentId);

            return Ok(new ApiOkResponse(student));
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
    [ClaimRequirement(EFunctionCode.GENERAL_STUDENT, ECommandCode.CREATE)]
    [ApiValidationFilter]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest request)
    {
        try
        {
            var student = await _studentsService.CreateStudentAsync(request);

            return Ok(new ApiCreatedResponse(student));
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
    
    [HttpPut("{studentId}")]
    [ClaimRequirement(EFunctionCode.GENERAL_STUDENT, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateStudent(string studentId, [FromBody] UpdateStudentRequest request)
    {
        try
        {
            await _studentsService.UpdateStudentAsync(studentId, request);

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
    
    [HttpDelete("{studentId}")]
    [ClaimRequirement(EFunctionCode.GENERAL_STUDENT, ECommandCode.DELETE)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteStudent(string studentId)
    {
        try
        {
            await _studentsService.DeleteStudentAsync(studentId);

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