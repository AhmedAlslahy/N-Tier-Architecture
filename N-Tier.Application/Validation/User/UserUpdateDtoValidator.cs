using FluentValidation;

namespace N_Tier.Application.Validation.User;

public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator()
    {
        RuleFor(x => x.FullName)
            .MaximumLength(100)
            .WithMessage("Full name cannot exceed 100 characters.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[0-9]{10,15}$")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("Invalid phone number format.");

        RuleFor(x => x.ProfileDescription)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.ProfileDescription))
            .WithMessage("Profile description cannot exceed 500 characters.");

        RuleFor(x => x.PublicLink)
            .MaximumLength(50)
            .WithMessage("Public link cannot exceed 50 characters.")
            .Matches(@"^[a-zA-Z0-9_-]+$")
            .WithMessage("Public link can only contain letters, numbers, underscores and hyphens.");

        RuleFor(x => x.Image)
            .Must(file =>
                file == null ||
                new[] { ".jpg", ".jpeg", ".png", ".webp" }
                .Contains(Path.GetExtension(file.FileName).ToLower()))
            .WithMessage("Only jpg, jpeg, png and webp images are allowed.");

        RuleFor(x => x.Image)
            .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
            .WithMessage("Image size must not exceed 5 MB.");
    }
}