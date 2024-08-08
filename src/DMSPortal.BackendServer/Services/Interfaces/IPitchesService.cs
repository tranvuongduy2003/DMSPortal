using DMSPortal.Models.DTOs.Pitch;
using DMSPortal.Models.Models;
using DMSPortal.Models.Requests.Pitch;

namespace DMSPortal.BackendServer.Services.Interfaces;

public interface IPitchesService
{
    Task<Pagination<PitchDto>> GetPitchesAsync(PaginationFilter filter);
    
    Task<Pagination<PitchDto>> GetPitchesByBranchIdAsync(string branchId, PaginationFilter filter);
    
    Task<bool> CreatePitchAsync(CreatePitchRequest request);
    
    Task<bool> UpdatePitchAsync(string pitchId, UpdatePitchRequest request);
    
    Task<bool> DeletePitchAsync(string pitchId);
}