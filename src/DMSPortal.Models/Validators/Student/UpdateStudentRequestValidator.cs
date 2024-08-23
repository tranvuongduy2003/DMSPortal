using DMSPortal.Models.Enums;
using DMSPortal.Models.Requests.Student;
using DMSPortal.Models.Validators.Note;
using FluentValidation;

namespace DMSPortal.Models.Validators.Student;

public class UpdateStudentRequestValidator : AbstractValidator<UpdateStudentRequest>
{
    public UpdateStudentRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id không được để trống")
            .MaximumLength(50)
            .WithMessage("Id phải có ít hơn 50 kí tự");
        
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("FullName không được để trống")
            .MaximumLength(50)
            .WithMessage("FullName phải có ít hơn 50 kí tự");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("PhoneNumber phải có ít hơn 20 kí tự");

        RuleFor(x => x.DOB)
            .NotEmpty()
            .WithMessage("DOB không được để trống");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address không được để trống")
            .MaximumLength(255)
            .WithMessage("Address phải có ít hơn 255 kí tự");

        RuleFor(x => x.Gender)
            .IsInEnum()
            .When(x => x.Gender != null)
            .WithMessage($"Gender phải là {nameof(EGender.MALE)}, {nameof(EGender.FEMALE)} hoặc {nameof(EGender.OTHER)}");

        RuleFor(x => x.Height)
            .ExclusiveBetween(0, 300)
            .When(x => x.Height != null)
            .WithMessage("Height phải trong khoảng từ 0 to 300");

        RuleFor(x => x.Weight)
            .ExclusiveBetween(0, 300)
            .When(x => x.Weight != null)
            .WithMessage("Weight phải trong khoảng từ 0 to 300");

        RuleFor(x => x.FavouritePosition)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.FavouritePosition))
            .WithMessage("FavouritePosition phải có ít hơn 255 kí tự");

        RuleFor(x => x.FatherFullName)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.FatherFullName))
            .WithMessage("FatherFullName phải có ít hơn 50 kí tự");

        RuleFor(x => x.FatherBirthYear)
            .ExclusiveBetween(1000, DateTime.Now.Year)
            .When(x => x.FatherBirthYear != null)
            .WithMessage($"FatherBirthYear phải trong khoảng từ 1000 tới {DateTime.Now.Year}");

        RuleFor(x => x.FatherAddress)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.FatherAddress))
            .WithMessage("FatherAddress phải có ít hơn 255 kí tự");

        RuleFor(x => x.FatherPhoneNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.FatherPhoneNumber))
            .WithMessage("FatherPhoneNumber phải có ít hơn 20 kí tự");

        RuleFor(x => x.FatherEmail)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.FatherEmail))
            .WithMessage("FatherEmail phải là một email")
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.FatherEmail))
            .WithMessage("FatherEmail phải có ít hơn 50 kí tự");

        RuleFor(x => x.MotherFullName)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.MotherFullName))
            .WithMessage("MotherFullName phải có ít hơn 50 kí tự");

        RuleFor(x => x.MotherBirthYear)
            .ExclusiveBetween(1000, DateTime.Now.Year)
            .When(x => x.MotherBirthYear != null)
            .WithMessage($"MotherBirthYear phải trong khoảng từ 1000 tới {DateTime.Now.Year}");

        RuleFor(x => x.MotherAddress)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.MotherAddress))
            .WithMessage("MotherAddress phải có ít hơn 255 kí tự");

        RuleFor(x => x.MotherPhoneNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.MotherPhoneNumber))
            .WithMessage("MotherPhoneNumber phải có ít hơn 20 kí tự");

        RuleFor(x => x.MotherEmail)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.MotherEmail))
            .WithMessage("MotherEmail phải là một email")
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.MotherEmail))
            .WithMessage("MotherEmail phải có ít hơn 50 kí tự");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status không được để trống")
            .IsInEnum()
            .WithMessage(
                $"Status phải là {nameof(EStudentStatus.INACTIVE)}, {nameof(EStudentStatus.ACTIVE)}, {nameof(EStudentStatus.ENROLLED)}, {nameof(EStudentStatus.SUSPENDED)} hoặc {nameof(EStudentStatus.WAITLISTED)}");
    }
}