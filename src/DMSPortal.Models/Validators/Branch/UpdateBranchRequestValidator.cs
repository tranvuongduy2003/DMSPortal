using DMSPortal.Models.Enums;
using DMSPortal.Models.Requests.Branch;
using FluentValidation;

namespace DMSPortal.Models.Validators.Branch;

public class UpdateBranchRequestValidator : AbstractValidator<UpdateBranchRequest>
{
    public UpdateBranchRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id không được để trống")
            .MaximumLength(50)
            .WithMessage("Id phải có ít hơn 50 kí tự");
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name không được để trống");
        
        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address không được để trống");
        
        RuleFor(x => x.ManagerId)
            .NotEmpty()
            .WithMessage("ManagerId không được để trống")
            .MaximumLength(50)
            .WithMessage("ManagerId phải có ít hơn 50 kí tự");
        
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status không được để trống")
            .IsInEnum()
            .WithMessage($"Status phải là {nameof(EBranchStatus.FULL)}, {nameof(EBranchStatus.OPEN)}, {nameof(EBranchStatus.CLOSED)} hoặc {nameof(EBranchStatus.UNDER_MAINTENANCE)}");
    }
}