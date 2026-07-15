using FluentValidation;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Application.Helper.User;

namespace N_Tier.Application.Features.Auth;

public static class Register
{
    public sealed class RegisterReq
    {
        public required string FullName { get; init; }
        public required string UserName { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required string ConfirmPassword { get; init; }
    }

    public sealed class Validator : AbstractValidator<RegisterReq>
    {
        public Validator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.UserName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(20)
                .Matches("^[a-zA-Z0-9_]+$");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]")
                .Matches("[a-z]")
                .Matches("[0-9]");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password);
        }
    }

    public sealed class RegisterHandler(SarhneDbContext context)
    {
        public async Task<Result> Handle(RegisterReq req, CancellationToken cancellationToken = default)
        {
            var existingUser = await context.Users.FindByEmailAsync(req.Email);
            if (existingUser != null)
                return UserErrors.AlreadyExists;

            var user = new Core.Identity.User
            {
                Email = req.Email,
                FullName = req.FullName,
                UserName = req.UserName,
                PasswordHashed = PasswordService.HashPassword(req.Password),
                EmailConfirmed = false,
                UserSetting = new Core.Entities.UserSetting()
            };

            await context.Users.AddAsync(user, cancellationToken);
            await context.AddRoleAsync(user, "User");
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}