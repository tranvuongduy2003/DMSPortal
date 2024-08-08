using DMSPortal.Models.Requests;
using FluentValidation;

namespace DMSPortal.Models.Validators.Permission;

public class UpdatePermissionByCommandRequestValidator : AbstractValidator<UpdatePermissionByCommandRequest>
{
    public UpdatePermissionByCommandRequestValidator()
    {
        RuleFor(x => x.CommandId)
            .NotEmpty()
            .WithMessage("CommandId is required")
            .MaximumLength(50)
            .WithMessage("Id must be less than 50 characters");
        
        RuleFor(x => x.FunctionId)
            .NotEmpty()
            .WithMessage("FunctionId is required")
            .MaximumLength(50)
            .WithMessage("Id must be less than 50 characters");
        
        RuleFor(x => x.Value)
            .NotNull()
            .WithMessage("Value is required");
    }
}