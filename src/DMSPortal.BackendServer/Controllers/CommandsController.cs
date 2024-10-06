using DMSPortal.BackendServer.Abstractions.Services;
using DMSPortal.BackendServer.Abstractions.UseCases;
using DMSPortal.BackendServer.Attributes;
using DMSPortal.Models.DTOs.Command;
using DMSPortal.Models.Enums;
using DMSPortal.Models.HttpResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandsUseCase _commandsUseCase;

    public CommandsController(ICommandsUseCase commandsUseCase)
    {
        _commandsUseCase = commandsUseCase;
    }

    [HttpGet]
    [ClaimRequirement(EFunctionCode.SYSTEM_COMMAND, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(List<CommandDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCommands()
    {
        var commandDtos = await _commandsUseCase.GetAllCommandsAsync();

        return Ok(new ApiOkResponse(commandDtos));
    }
}