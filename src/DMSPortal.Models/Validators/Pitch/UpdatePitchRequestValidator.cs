using DMSPortal.Models.Enums;
using DMSPortal.Models.Requests.Pitch;
using FluentValidation;

namespace DMSPortal.Models.Validators.Pitch;

public class UpdatePitchRequestValidator : AbstractValidator<UpdatePitchRequest>
{
    public UpdatePitchRequestValidator()
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
            .WithMessage($"Status must be {nameof(EPitchStatus.UNDER_MAINTENANCE)}, {nameof(EPitchStatus.CLOSED)}, {nameof(EPitchStatus.BUSY)}, {nameof(EPitchStatus.AVAILABLE)} or {nameof(EPitchStatus.RESERVED)}");
    }
}