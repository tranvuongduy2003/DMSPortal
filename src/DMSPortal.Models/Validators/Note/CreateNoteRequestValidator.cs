using DMSPortal.Models.Enums;
using DMSPortal.Models.Requests.Note;
using FluentValidation;

namespace DMSPortal.Models.Validators.Note;

public class CreateNoteRequestValidator : AbstractValidator<CreateNoteRequest>
{
    public CreateNoteRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content không được để trống")
            .MaximumLength(1000)
            .WithMessage("Content phải có ít hơn 1000 kí tự");
    }
}