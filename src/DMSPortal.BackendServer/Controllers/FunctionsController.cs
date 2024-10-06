using DMSPortal.BackendServer.Abstractions.Services;
using DMSPortal.BackendServer.Abstractions.UseCases;
using DMSPortal.BackendServer.Attributes;
using DMSPortal.Models.DTOs.Function;
using DMSPortal.Models.Enums;
using DMSPortal.Models.HttpResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class FunctionsController : ControllerBase
{
    private readonly IFunctionsUseCase _functionsUseCase;

    public FunctionsController(IFunctionsUseCase functionsUseCase)
    {
        _functionsUseCase = functionsUseCase;
    }
    
    [HttpGet]
    [ClaimRequirement(EFunctionCode.SYSTEM_FUNCTION, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(List<FunctionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFunctions()
    {
        var functions = await _functionsUseCase.GetAllFunctionsAsync();

        return Ok(new ApiOkResponse(functions));
    }
}