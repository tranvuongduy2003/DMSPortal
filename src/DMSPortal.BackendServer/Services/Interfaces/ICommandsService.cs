using DMSPortal.Models.DTOs;

namespace DMSPortal.BackendServer.Services.Interfaces;

public interface ICommandsService
{
    Task<List<CommandDto>> GetAllCommandsAsync();
}