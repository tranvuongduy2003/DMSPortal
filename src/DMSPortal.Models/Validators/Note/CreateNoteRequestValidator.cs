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
            .WithMessage("Content is required")
            .MaximumLength(1000)
            .WithMessage("Content must be less than 1000 characters");
    }
}