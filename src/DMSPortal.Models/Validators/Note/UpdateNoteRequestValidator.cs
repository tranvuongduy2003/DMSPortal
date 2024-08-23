using DMSPortal.Models.Requests.Note;
using FluentValidation;

namespace DMSPortal.Models.Validators.Note;

public class UpdateNoteRequestValidator : AbstractValidator<UpdateNoteRequest>
{
    public UpdateNoteRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id không được để trống")
            .MaximumLength(50)
            .WithMessage("Id phải có ít hơn 50 kí tự");
        
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content không được để trống")
            .MaximumLength(1000)
            .WithMessage("Content phải có ít hơn 1000 kí tự");
    }
}