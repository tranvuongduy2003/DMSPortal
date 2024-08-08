using DMSPortal.Models.Requests.Note;
using FluentValidation;

namespace DMSPortal.Models.Validators.Note;

public class UpdateNoteRequestValidator : AbstractValidator<UpdateNoteRequest>
{
    public UpdateNoteRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required")
            .MaximumLength(50)
            .WithMessage("Id must be less than 50 characters");
        
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required")
            .MaximumLength(1000)
            .WithMessage("Content must be less than 1000 characters");
    }
}