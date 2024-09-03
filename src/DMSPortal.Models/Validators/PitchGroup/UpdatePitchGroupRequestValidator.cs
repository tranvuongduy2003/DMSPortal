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
            .WithMessage($"Status phải là {nameof(EPitchGroupStatus.FULL)}, {nameof(EPitchGroupStatus.INACTIVE)} hoặc {nameof(EPitchGroupStatus.AVAILABLE)}");
    }
}