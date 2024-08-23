using DMSPortal.BackendServer.Attributes;
using DMSPortal.BackendServer.Helpers.HttpResponses;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs.Function;
using DMSPortal.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class FunctionsController : ControllerBase
{
    private readonly IFunctionsService _functionsService;

    public FunctionsController(IFunctionsService functionsService)
    {
        _functionsService = functionsService;
    }
    
    [HttpGet]
    [ClaimRequirement(EFunctionCode.SYSTEM_FUNCTION, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(List<FunctionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFunctions()
    {
        var functions = await _functionsService.GetAllFunctionsAsync();

        return Ok(new ApiOkResponse(functions));
    }
}