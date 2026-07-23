using FluentValidation;
using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Application.Helper.Users;

namespace N_Tier.Application.Features.Auth;

public static class Register
{
    public sealed record Command(
        string FullName,
        string UserName,
        string Email,
        string Password,
        string ConfirmPassword
    ) : IRequest<Result>;

    public sealed class Validator : AbstractValidator<Command>
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

    public sealed class Handler(SarhneDbContext context)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(
            Command req,
            CancellationToken cancellationToken)
        {
            var existingUser = await context.Users.FindByEmailAsync(req.Email);

            if (existingUser != null)
                return UserErrors.AlreadyExists;

            var user = new Core.Entities.Identity.User
            {
                Email = req.Email,
                FullName = req.FullName,
                UserName = req.UserName,
                PasswordHashed = PasswordService.HashPassword(req.Password),
                EmailConfirmed = false,
                UserSetting = new UserSetting()
            };

            await context.Users.AddAsync(user, cancellationToken);
            await context.AddRoleAsync(user, "User");
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}