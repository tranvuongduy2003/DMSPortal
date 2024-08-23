using DMSPortal.BackendServer.Attributes;
using DMSPortal.BackendServer.Helpers.HttpResponses;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs.Command;
using DMSPortal.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandsService _commandsService;

    public CommandsController(ICommandsService commandsService)
    {
        _commandsService = commandsService;
    }

    [HttpGet]
    [ClaimRequirement(EFunctionCode.SYSTEM_COMMAND, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(List<CommandDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCommands()
    {
        var commandDtos = await _commandsService.GetAllCommandsAsync();

        return Ok(new ApiOkResponse(commandDtos));
    }
}