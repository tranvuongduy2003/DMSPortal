using DMSPortal.Models.Requests;
using FluentValidation;

namespace DMSPortal.Models.Validators.Permission;

public class UpdatePermissionByCommandRequestValidator : AbstractValidator<UpdatePermissionByCommandRequest>
{
    public UpdatePermissionByCommandRequestValidator()
    {
        RuleFor(x => x.CommandId)
            .NotEmpty()
            .WithMessage("CommandId không được để trống")
            .MaximumLength(50)
            .WithMessage("Id phải có ít hơn 50 kí tự");
        
        RuleFor(x => x.FunctionId)
            .NotEmpty()
            .WithMessage("FunctionId không được để trống")
            .MaximumLength(50)
            .WithMessage("Id phải có ít hơn 50 kí tự");
        
        RuleFor(x => x.Value)
            .NotNull()
            .WithMessage("Value không được để trống");
    }
}