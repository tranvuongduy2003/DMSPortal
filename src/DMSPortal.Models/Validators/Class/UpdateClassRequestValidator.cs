using DMSPortal.Models.Enums;
using DMSPortal.Models.Requests.Class;
using FluentValidation;

namespace DMSPortal.Models.Validators.Class;

public class UpdateClassRequestValidator : AbstractValidator<UpdateClassRequest>
{
    public UpdateClassRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id không được để trống")
            .MaximumLength(50)
            .WithMessage("Id phải có ít hơn 50 kí tự");
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name không được để trống");
        
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status không được để trống")
            .IsInEnum()
            .WithMessage($"Status phải là {nameof(EClassStatus.FULL)}, {nameof(EClassStatus.CANCELED)}, {nameof(EClassStatus.COMPLETED)}, {nameof(EClassStatus.IN_PROGRESS)}, {nameof(EClassStatus.POSTPONED)}, {nameof(EClassStatus.SCHEDULED)} hoặc {nameof(EClassStatus.OPEN_FOR_ENROLLMENT)}");
    }
}