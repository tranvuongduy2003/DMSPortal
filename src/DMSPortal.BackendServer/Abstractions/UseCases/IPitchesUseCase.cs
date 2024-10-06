using DMSPortal.Models.Common;
using DMSPortal.Models.DTOs.Pitch;
using DMSPortal.Models.Requests.Pitch;

namespace DMSPortal.BackendServer.Abstractions.UseCases;

public interface IPitchesUseCase
{
    Task<Pagination<PitchDto>> GetPitchesAsync(PaginationFilter filter);
    
    Task<Pagination<PitchDto>> GetPitchesByBranchIdAsync(string branchId, PaginationFilter filter);
    
    Task<PitchDto> GetPitchByIdAsync(string pitchId);
    
    Task<PitchDto> CreatePitchAsync(CreatePitchRequest request);
    
    Task<bool> UpdatePitchAsync(string pitchId, UpdatePitchRequest request);
    
    Task<bool> DeletePitchAsync(string pitchId);
}