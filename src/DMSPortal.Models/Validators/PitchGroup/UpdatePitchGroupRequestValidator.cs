using DMSPortal.Models.Enums;
using DMSPortal.Models.Requests.PitchGroup;
using FluentValidation;

namespace DMSPortal.Models.Validators.PitchGroup;

public class UpdatePitchGroupRequestValidator: AbstractValidator<UpdatePitchGroupRequest>
{
    public UpdatePitchGroupRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required")
            .MaximumLength(50)
            .WithMessage("Id must be less than 50 characters");
        
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