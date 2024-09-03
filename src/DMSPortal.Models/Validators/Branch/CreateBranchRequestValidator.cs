using DMSPortal.Models.Enums;
using DMSPortal.Models.Requests.Branch;
using FluentValidation;

namespace DMSPortal.Models.Validators.Branch;

public class CreateBranchRequestValidator : AbstractValidator<CreateBranchRequest>
{
    public CreateBranchRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name không được để trống");
        
        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address không được để trống");
        
        RuleFor(x => x.PitchGroupId)
            .NotEmpty()
            .WithMessage("PitchGroupId không được để trống")
            .MaximumLength(50)
            .WithMessage("PitchGroupId phải có ít hơn 50 kí tự");
        
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