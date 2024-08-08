using DMSPortal.Models.Enums;
using DMSPortal.Models.Requests.PitchGroup;
using FluentValidation;

namespace DMSPortal.Models.Validators.PitchGroup;

public class CreatePitchGroupRequestValidator: AbstractValidator<CreatePitchGroupRequest>
{
    public CreatePitchGroupRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");
        
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .IsInEnum()
            .WithMessage($"Status must be {nameof(EPitchGroupStatus.FULL)}, {nameof(EPitchGroupStatus.INACTIVE)} or {nameof(EPitchGroupStatus.AVAILABLE)}");
    }
}