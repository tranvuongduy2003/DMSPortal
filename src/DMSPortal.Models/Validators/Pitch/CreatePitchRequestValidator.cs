using DMSPortal.Models.Enums;
using DMSPortal.Models.Requests.Pitch;
using FluentValidation;

namespace DMSPortal.Models.Validators.Pitch;

public class CreatePitchRequestValidator : AbstractValidator<CreatePitchRequest>
{
    public CreatePitchRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");
        
        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("BranchId is required")
            .MaximumLength(50)
            .WithMessage("BranchId must be less than 50 characters");
        
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .IsInEnum()
            .WithMessage($"Status must be {nameof(EPitchStatus.UNDER_MAINTENANCE)}, {nameof(EPitchStatus.CLOSED)}, {nameof(EPitchStatus.BUSY)}, {nameof(EPitchStatus.AVAILABLE)} or {nameof(EPitchStatus.RESERVED)}");
    }
}