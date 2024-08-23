using DMSPortal.BackendServer.Models;
using DMSPortal.Models.DTOs.Branch;
using DMSPortal.Models.Requests.Branch;

namespace DMSPortal.BackendServer.Services.Interfaces;

public interface IBranchesService
{
    Task<Pagination<BranchDto>> GetBranchesAsync(PaginationFilter filter);
    
    Task<Pagination<BranchDto>> GetBranchesByPitchGroupIdAsync(string pitchGroupId, PaginationFilter filter);
    
    Task<BranchDto> GetBranchByIdAsync(string branchId);
    
    Task<BranchDto> CreateBranchAsync(CreateBranchRequest request);
    
    Task<bool> UpdateBranchAsync(string branchId, UpdateBranchRequest request);
    
    Task<bool> DeleteBranchAsync(string branchId);
}