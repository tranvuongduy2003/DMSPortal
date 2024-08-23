using DMSPortal.Models.Requests;
using FluentValidation;

namespace DMSPortal.Models.Validators.Permission;

public class UpdatePermissionByRoleRequestValidator : AbstractValidator<UpdatePermissionByRoleRequest>
{
    public UpdatePermissionByRoleRequestValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("RoleId không được để trống");
        
        RuleFor(x => x.FunctionId)
            .NotEmpty()
            .WithMessage("FunctionId không được để trống");
        
        RuleFor(x => x.Value)
            .NotNull()
            .WithMessage("Value không được để trống");
    }
}