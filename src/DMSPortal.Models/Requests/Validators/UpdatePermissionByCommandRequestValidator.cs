﻿using FluentValidation;

namespace DMSPortal.Models.Requests.Validators;

public class UpdatePermissionByCommandRequestValidator : AbstractValidator<UpdatePermissionByCommandRequest>
{
    public UpdatePermissionByCommandRequestValidator()
    {
        RuleFor(x => x.CommandId)
            .NotEmpty()
            .WithMessage("CommandId is required");
        
        RuleFor(x => x.FunctionId)
            .NotEmpty()
            .WithMessage("FunctionId is required");
        
        RuleFor(x => x.Value)
            .NotNull()
            .WithMessage("Value is required");
    }
}