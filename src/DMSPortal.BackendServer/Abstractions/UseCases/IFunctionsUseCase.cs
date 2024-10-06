using DMSPortal.Models.DTOs.Function;

namespace DMSPortal.BackendServer.Abstractions.UseCases;

public interface IFunctionsUseCase
{
    Task<List<FunctionDto>> GetAllFunctionsAsync();
}