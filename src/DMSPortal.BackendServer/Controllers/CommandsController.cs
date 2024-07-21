using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Helpers.HttpResponses;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Controllers;

[Route("api/commands")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandsService _commandsService;

    public CommandsController(ICommandsService commandsService)
    {
        _commandsService = commandsService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CommandDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCommands()
    {
        var commandDtos = await _commandsService.GetAllCommandsAsync();

        return Ok(new ApiOkResponse(commandDtos));
    }
}