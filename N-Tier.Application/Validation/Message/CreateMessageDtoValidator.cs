using FluentValidation;

namespace N_Tier.Application.Validation.Message
{
    public class CreateMessageDtoValidator : AbstractValidator<CreateMessageDto>
    {
        public CreateMessageDtoValidator()
        {
            RuleFor(x => x.ReceiverId)
                .NotEmpty()
                .WithMessage("ReceiverId is required.");

            RuleFor(x => x)
                .Must(x =>
                    !string.IsNullOrWhiteSpace(x.Content) ||
                    x.Photo != null)
                .WithMessage("Message must contain content or a photo.");

            RuleFor(x => x.Content)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.Content));

            RuleFor(x => x.Photo)
                .Must(file =>
                    file == null ||
                    new[] { ".jpg", ".jpeg", ".png", ".webp" }
                        .Contains(Path.GetExtension(file.FileName).ToLower()))
                .WithMessage("Only jpg, jpeg, png and webp images are allowed.");

            RuleFor(x => x.Photo)
                .Must(file =>
                    file == null ||
                    file.Length <= 5 * 1024 * 1024)
                .WithMessage("Photo size must not exceed 5 MB.");
        }
    }
}