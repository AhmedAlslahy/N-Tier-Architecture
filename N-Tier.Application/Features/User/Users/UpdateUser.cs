using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Application.Helper.Files;
using N_Tier.Shared.Service;

namespace N_Tier.Application.Features.User.Users;

public static class UpdateUser
{
    public sealed class Command : IRequest<Result>
    {
        public required string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileDescription { get; set; }
        public string? PublicLink { get; set; }
        public IFormFile? Image { get; set; }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.FullName)
                .MaximumLength(100);

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[0-9]{10,15}$")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

            RuleFor(x => x.ProfileDescription)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.ProfileDescription));

            RuleFor(x => x.PublicLink)
                .MaximumLength(50)
                .Matches(@"^[a-zA-Z0-9_-]+$");

            RuleFor(x => x.Image)
                .Must(file =>
                    file == null ||
                    new[] { ".jpg", ".jpeg", ".png", ".webp" }
                        .Contains(Path.GetExtension(file.FileName).ToLower()));

            RuleFor(x => x.Image)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024);
        }
    }

    public sealed class Handler(
        SarhneDbContext context,
        ICurrentUserService currentUser)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(
            Command request,
            CancellationToken cancellationToken)
        {
            var user = await context.Users.FindAsync(
                [currentUser.UserId],
                cancellationToken);

            if (user is null)
                return UserErrors.NotFound;

            user.FullName = request.FullName;
            user.PhoneNumber = request.PhoneNumber;
            user.ProfileDescription = request.ProfileDescription;

            if (!string.IsNullOrWhiteSpace(request.PublicLink))
            {
                var uniqueNumber = RandomNumberGenerator
                    .GetInt32(1000, 9999)
                    .ToString();

                user.PublicLink = request.PublicLink + uniqueNumber;
            }

            if (request.Image is not null)
            {
                user.ImageUrl = Upload.UploadFile("Photos", request.Image);
            }

            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}