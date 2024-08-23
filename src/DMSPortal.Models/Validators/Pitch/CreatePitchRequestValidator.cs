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
            .WithMessage("Name không được để trống");
        
        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("BranchId không được để trống")
            .MaximumLength(50)
            .WithMessage("BranchId phải có ít hơn 50 kí tự");
        
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status không được để trống")
            .IsInEnum()
            .WithMessage($"Status phải là {nameof(EPitchStatus.UNDER_MAINTENANCE)}, {nameof(EPitchStatus.CLOSED)}, {nameof(EPitchStatus.BUSY)}, {nameof(EPitchStatus.AVAILABLE)} hoặc {nameof(EPitchStatus.RESERVED)}");
    }
}