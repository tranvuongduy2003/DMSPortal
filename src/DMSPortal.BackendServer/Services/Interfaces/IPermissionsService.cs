using DMSPortal.Models.DTOs;
using DMSPortal.Models.Requests;

namespace DMSPortal.BackendServer.Services.Interfaces;

public interface IPermissionsService
{
    Task<List<PermissionScreenDto>> GetCommandViewsAsync();

    Task<List<RolePermissionDto>> GetRolePermissionsAsync();

    Task<bool> UpdatePermissionByCommand(UpdatePermissionByCommandRequest request);
    
    Task<bool> UpdatePermissionByRole(UpdatePermissionByRoleRequest request);
}