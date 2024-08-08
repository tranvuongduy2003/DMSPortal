using DMSPortal.Models.DTOs.Command;

namespace DMSPortal.BackendServer.Services.Interfaces;

public interface ICommandsService
{
    Task<List<CommandDto>> GetAllCommandsAsync();
}