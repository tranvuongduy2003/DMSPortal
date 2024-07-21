﻿using System.Data;
using Dapper;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Helpers.HttpResponses;
using DMSPortal.BackendServer.Infrastructure.Interfaces;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs;
using DMSPortal.Models.Enums;
using DMSPortal.Models.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DMSPortal.BackendServer.Services;

public class PermissionsService : IPermissionsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly RoleManager<Role> _roleManager;

    public PermissionsService(IUnitOfWork unitOfWork, RoleManager<Role> roleManager)
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
                throw new KeyNotFoundException("Funtion does not exist!");

            var command = await _unitOfWork.Commands.GetByIdAsync(request.CommandId);
            if (command == null)
                throw new KeyNotFoundException("Command does not exist!");

            var commandInFunction = await _unitOfWork.CommandInFunctions
                .FindByCondition(cif =>
                    cif.FunctionId.Equals(request.FunctionId) 
                    && cif.CommandId.Equals(request.CommandId))
                .FirstOrDefaultAsync();
            
            if (commandInFunction != null)
                throw new BadHttpRequestException("Permission already exists!");
            
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
                throw new KeyNotFoundException("Funtion does not exist!");
    
            var role = await _roleManager.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id.Equals(request.RoleId));
            if (role == null)
                throw new KeyNotFoundException("Role does not exist!");

            var permissions = await _unitOfWork.Permissions
                .FindByCondition(p => p.FunctionId.Equals(request.FunctionId) 
                                      && p.RoleId.Equals(request.RoleId)).ToListAsync();
    
            if (request.Value == true)
            {
                if (permissions.Count > 0)
                    throw new BadHttpRequestException("Permission already exists!");
                
                permissions = await _unitOfWork.CommandInFunctions
                    .FindByCondition(cif => cif.FunctionId.Equals(request.FunctionId))
                    .Select(cif => new Permission(cif.FunctionId, request.RoleId, cif.CommandId))
                    .ToListAsync();
    
                await _unitOfWork.Permissions.CreateListAsync(permissions);
            }
            else
            {
                if (permissions.Count <= 0)
                    throw new BadHttpRequestException("Permission does not exist!");
    
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