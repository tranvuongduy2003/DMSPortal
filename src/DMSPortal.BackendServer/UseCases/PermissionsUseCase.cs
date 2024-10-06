using DMSPortal.BackendServer.Abstractions.UnitOfWork;
using DMSPortal.BackendServer.Abstractions.UseCases;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.Models.DTOs.Permission;
using DMSPortal.Models.Enums;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DMSPortal.BackendServer.UseCases;

public class PermissionsUseCase : IPermissionsUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly RoleManager<Role> _roleManager;

    public PermissionsUseCase(IUnitOfWork unitOfWork, RoleManager<Role> roleManager)
    {
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
    }

    public async Task<List<PermissionScreenDto>> GetCommandViewsAsync()
    {
        var functions = _unitOfWork.Functions.FindAll();

        var permissions = await functions
            .Include(f => f.CommandInFunctions)
            .Select(f => new PermissionScreenDto
            {
                Id = f.Id,
                Name = f.Name,
                ParentId = f.ParentId,
                HasView = f.CommandInFunctions.Any(x => x.CommandId.Equals(nameof(ECommandCode.VIEW))),
                HasCreate = f.CommandInFunctions.Any(x => x.CommandId.Equals(nameof(ECommandCode.CREATE))),
                HasUpdate = f.CommandInFunctions.Any(x => x.CommandId.Equals(nameof(ECommandCode.UPDATE))),
                HasDelete = f.CommandInFunctions.Any(x => x.CommandId.Equals(nameof(ECommandCode.DELETE))),
            })
            .ToListAsync();

        return permissions;
    }

    public async Task<List<RolePermissionDto>> GetRolePermissionsAsync()
    {
        var roles = _roleManager.Roles.AsNoTracking();

        var rolePermissions = await roles
            .Include(x => x.Permissions)
            .ThenInclude(x => x.Function)
            .Select(r => new RolePermissionDto
            {
                RoleId = r.Id,
                RoleName = r.Name,
                FunctionIds = r.Permissions.Select(p => p.FunctionId).ToList(),
                FunctionNames = r.Permissions.Select(p => p.Function.Name).ToList(),
            })
            .ToListAsync();

        return rolePermissions;
    }

    public async Task<bool> UpdatePermissionByCommand(UpdatePermissionByCommandRequest request)
    {
        try
        {
            var function = await _unitOfWork.Functions.GetByIdAsync(request.FunctionId);
            if (function == null)
                throw new NotFoundException("Chức năng không tồn tại!");

            var command = await _unitOfWork.Commands.GetByIdAsync(request.CommandId);
            if (command == null)
                throw new NotFoundException("Thao tác không tồn tại!");

            var commandInFunction = await _unitOfWork.CommandInFunctions
                .FindByCondition(cif =>
                    cif.FunctionId.Equals(request.FunctionId) 
                    && cif.CommandId.Equals(request.CommandId))
                .FirstOrDefaultAsync();
            
            if (commandInFunction != null)
                throw new BadRequestException("Quyền đã tồn tại!");
            
            if (request.Value == true)
            {
                commandInFunction = new CommandInFunction
                {
                    CommandId = request.CommandId,
                    FunctionId = request.FunctionId
                };

                await _unitOfWork.CommandInFunctions.CreateAsync(commandInFunction);
            }
            else
            {
                await _unitOfWork.CommandInFunctions.DeleteAsync(commandInFunction);
            }
            
            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> UpdatePermissionByRole(UpdatePermissionByRoleRequest request)
    {
        try
        {
            var function = await _unitOfWork.Functions.GetByIdAsync(request.FunctionId);
            if (function == null)
                throw new NotFoundException("Chức năng đã tổn tại!");
    
            var role = await _roleManager.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id.Equals(request.RoleId));
            if (role == null)
                throw new NotFoundException("Vai trò không tồn tại!");

            var permissions = await _unitOfWork.Permissions
                .FindByCondition(p => p.FunctionId.Equals(request.FunctionId) 
                                      && p.RoleId.Equals(request.RoleId)).ToListAsync();
    
            if (request.Value == true)
            {
                if (permissions.Count > 0)
                    throw new BadRequestException("Quyền đã tổn tại!");
                
                permissions = await _unitOfWork.CommandInFunctions
                    .FindByCondition(cif => cif.FunctionId.Equals(request.FunctionId))
                    .Select(cif => new Permission(cif.FunctionId, request.RoleId, cif.CommandId))
                    .ToListAsync();
    
                await _unitOfWork.Permissions.CreateListAsync(permissions);
            }
            else
            {
                if (permissions.Count <= 0)
                    throw new BadRequestException("Quyền không tồn tại!");
    
                await _unitOfWork.Permissions.DeleteListAsync(permissions);
            }

            await _unitOfWork.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
}