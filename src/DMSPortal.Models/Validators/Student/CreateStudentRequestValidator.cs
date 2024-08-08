using DMSPortal.Models.Enums;
using DMSPortal.Models.Requests.Student;
using DMSPortal.Models.Validators.Note;
using FluentValidation;

namespace DMSPortal.Models.Validators.Student;

public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
{
    public CreateStudentRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("FullName is required")
            .MaximumLength(50)
            .WithMessage("FullName must be less than 50 characters");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("PhoneNumber must be less than 20 characters");

        RuleFor(x => x.DOB)
            .NotEmpty()
            .WithMessage("DOB is required");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MaximumLength(255)
            .WithMessage("Address must be less than 255 characters");

        RuleFor(x => x.Gender)
            .IsInEnum()
            .When(x => x.Gender != null)
            .WithMessage($"Gender must be {nameof(EGender.MALE)}, {nameof(EGender.FEMALE)} or {nameof(EGender.OTHER)}");

        RuleFor(x => x.Height)
            .ExclusiveBetween(0, 300)
            .When(x => x.Height != null)
            .WithMessage("Height must be in range from 0 to 300");

        RuleFor(x => x.Weight)
            .ExclusiveBetween(0, 300)
            .When(x => x.Weight != null)
            .WithMessage("Weight must be in range from 0 to 300");

        RuleFor(x => x.FavouritePosition)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.FavouritePosition))
            .WithMessage("FavouritePosition must be less than 255 characters");

        RuleFor(x => x.FatherFullName)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.FatherFullName))
            .WithMessage("FatherFullName must be less than 50 characters");

        RuleFor(x => x.FatherBirthYear)
            .ExclusiveBetween(1000, DateTime.Now.Year)
            .When(x => x.FatherBirthYear != null)
            .WithMessage($"FatherBirthYear must be in range from 1000 to {DateTime.Now.Year}");

        RuleFor(x => x.FatherAddress)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.FatherAddress))
            .WithMessage("FatherAddress must be less than 255 characters");

        RuleFor(x => x.FatherPhoneNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.FatherPhoneNumber))
            .WithMessage("FatherPhoneNumber must be less than 20 characters");

        RuleFor(x => x.FatherEmail)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.FatherEmail))
            .WithMessage("FatherEmail must be an email")
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.FatherEmail))
            .WithMessage("FatherEmail must be less than 50 characters");

        RuleFor(x => x.MotherFullName)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.MotherFullName))
            .WithMessage("MotherFullName must be less than 50 characters");

        RuleFor(x => x.MotherBirthYear)
            .ExclusiveBetween(1000, DateTime.Now.Year)
            .When(x => x.MotherBirthYear != null)
            .WithMessage($"MotherBirthYear must be in range from 1000 to {DateTime.Now.Year}");

        RuleFor(x => x.MotherAddress)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.MotherAddress))
            .WithMessage("MotherAddress must be less than 255 characters");

        RuleFor(x => x.MotherPhoneNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.MotherPhoneNumber))
            .WithMessage("MotherPhoneNumber must be less than 20 characters");

        RuleFor(x => x.MotherEmail)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.MotherEmail))
            .WithMessage("MotherEmail must be an email")
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.MotherEmail))
            .WithMessage("MotherEmail must be less than 50 characters");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .IsInEnum()
            .WithMessage(
                $"Status must be {nameof(EStudentStatus.INACTIVE)}, {nameof(EStudentStatus.ACTIVE)}, {nameof(EStudentStatus.ENROLLED)}, {nameof(EStudentStatus.SUSPENDED)} or {nameof(EStudentStatus.WAITLISTED)}");

        RuleFor(x => x.Note)
            .SetValidator(new CreateNoteRequestValidator()!);
    }
}