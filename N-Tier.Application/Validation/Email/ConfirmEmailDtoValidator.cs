using FluentValidation;

namespace N_Tier.Application.Validation.Email;

public class ConfirmEmailDtoValidator : AbstractValidator<ConfirmEmailDto>
{
    public ConfirmEmailDtoValidator()
    {
        RuleFor(x => x.OTP)
        .NotEmpty()
        .WithMessage("OTP is required.")
        .Matches(@"^\d{6}$");
    }
}