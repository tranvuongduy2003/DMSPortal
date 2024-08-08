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
            .WithMessage("Name is required");
        
        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required");
        
        RuleFor(x => x.PitchGroupId)
            .NotEmpty()
            .WithMessage("PitchGroupId is required")
            .MaximumLength(50)
            .WithMessage("PitchGroupId must be less than 50 characters");
        
        RuleFor(x => x.ManagerId)
            .NotEmpty()
            .WithMessage("ManagerId is required")
            .MaximumLength(50)
            .WithMessage("ManagerId must be less than 50 characters");
        
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .IsInEnum()
            .WithMessage($"Status must be {nameof(EBranchStatus.FULL)}, {nameof(EBranchStatus.OPEN)}, {nameof(EBranchStatus.CLOSED)} or {nameof(EBranchStatus.UNDER_MAINTENANCE)}");
    }
}