using FluentValidation;
using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;

namespace N_Tier.Application.Features.Admin.Roles;

public static class DeleteRole
{
    public sealed record Command(int UserId) : IRequest<Result>;

    public sealed class Handler(SarhneDbContext context) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command req, CancellationToken cancellationToken = default)
        {
            var rows = await context.Roles
        .Where(r => r.Id == req.UserId)
        .ExecuteDeleteAsync(cancellationToken);

            if (rows == 0)
            {
                return RoleErrors.NotFound;
            }

            return Result.Success();
        }
    }
}