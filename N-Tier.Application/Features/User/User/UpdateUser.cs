using FluentValidation;
using Microsoft.AspNetCore.Http;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Application.Helper.Files;

namespace N_Tier.Application.Features.User.User;

public static class UpdateUser
{
    public sealed class UpdateUserReq
    {
        public required string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileDescription { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string? PublicLink { get; set; }
    }

    public sealed class Validator : AbstractValidator<UpdateUserReq>
    {
        public Validator()
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

    public sealed class UpdateUserHandler(SarhneDbContext context)
    {
        public async Task<Result> Handle(UpdateUserReq req, int userId, CancellationToken cancellation = default)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return UserErrors.NotFound;
            }

            user.FullName = req.FullName!;
            user.ProfileDescription = req.ProfileDescription;
            user.PhoneNumber = req.PhoneNumber;
            var uniqueNumber = RandomNumberGenerator.GetInt32(1000, 9999).ToString();
            user.PublicLink = req.PublicLink + uniqueNumber;
            if (req.Image != null)
            {
                user.ImageUrl = Upload.UploadFile("Photos", req.Image);
            }

            await context.SaveChangesAsync(cancellation);
            return Result.Success();
        }
    }
}