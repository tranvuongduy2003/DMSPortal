using DMSPortal.Models.DTOs.Command;

namespace DMSPortal.BackendServer.Abstractions.UseCases;

public interface ICommandsUseCase
{
    Task<List<CommandDto>> GetAllCommandsAsync();
}