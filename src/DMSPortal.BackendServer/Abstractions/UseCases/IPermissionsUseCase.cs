using DMSPortal.Models.DTOs.Permission;
using DMSPortal.Models.Requests;

namespace DMSPortal.BackendServer.Abstractions.UseCases;

public interface IPermissionsUseCase
{
    Task<List<PermissionScreenDto>> GetCommandViewsAsync();

    Task<List<RolePermissionDto>> GetRolePermissionsAsync();

    Task<bool> UpdatePermissionByCommand(UpdatePermissionByCommandRequest request);
    
    Task<bool> UpdatePermissionByRole(UpdatePermissionByRoleRequest request);
}