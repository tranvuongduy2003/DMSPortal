using DMSPortal.Models.DTOs.Function;

namespace DMSPortal.BackendServer.Services.Interfaces;

public interface IFunctionsService
{
    Task<List<FunctionDto>> GetAllFunctionsAsync();
}