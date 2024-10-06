using DMSPortal.Models.Common;
using DMSPortal.Models.DTOs.PitchGroup;
using DMSPortal.Models.Requests.PitchGroup;

namespace DMSPortal.BackendServer.Abstractions.UseCases;

public interface IPitchGroupsUseCase
{
    Task<Pagination<PitchGroupDto>> GetPitchGroupsAsync(PaginationFilter filter);
    
    Task<PitchGroupDto> GetPitchGroupByIdAsync(string pitchGroupId);

    Task<PitchGroupDto> CreatePitchGroupAsync(CreatePitchGroupRequest request);
    
    Task<bool> UpdatePitchGroupAsync(string pitchGroupId, UpdatePitchGroupRequest request);
    
    Task<bool> DeletePitchGroupAsync(string pitchGroupId);
}