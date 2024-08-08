using DMSPortal.Models.Enums;
using DMSPortal.Models.Requests.Class;
using FluentValidation;

namespace DMSPortal.Models.Validators.Class;

public class CreateClassRequestValidator : AbstractValidator<CreateClassRequest>
{
    public CreateClassRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");
        
        RuleFor(x => x.PitchId)
            .NotEmpty()
            .WithMessage("PitchId is required")
            .MaximumLength(50)
            .WithMessage("PitchId must be less than 50 characters");
        
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .IsInEnum()
            .WithMessage($"Status must be {nameof(EClassStatus.FULL)}, {nameof(EClassStatus.CANCELED)}, {nameof(EClassStatus.COMPLETED)}, {nameof(EClassStatus.IN_PROGRESS)}, {nameof(EClassStatus.POSTPONED)}, {nameof(EClassStatus.SCHEDULED)} or {nameof(EClassStatus.OPEN_FOR_ENROLLMENT)}");
    }
}