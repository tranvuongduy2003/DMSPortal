﻿using DMSPortal.Models.Requests;
using FluentValidation;

namespace DMSPortal.Models.Validators.Permission;

public class UpdatePermissionByRoleRequestValidator : AbstractValidator<UpdatePermissionByRoleRequest>
{
    public UpdatePermissionByRoleRequestValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("RoleId is required");
        
        RuleFor(x => x.FunctionId)
            .NotEmpty()
            .WithMessage("FunctionId is required");
        
        RuleFor(x => x.Value)
            .NotNull()
            .WithMessage("Value is required");
    }
}