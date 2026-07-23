using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Core.Entities.Identity;

namespace N_Tier.Application.Features.Admin.Roles;

public static class CreateRole
{
    public sealed record Command(string RoleName) : IRequest<Result>;

    public sealed class Handler(SarhneDbContext context)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RoleName))
            {
                return RoleErrors.InvalidData;
            }

            var exists = await context.Roles
                .FirstOrDefaultAsync(
                    r => r.Name == request.RoleName,
                    cancellationToken);

            if (exists is not null)
            {
                return RoleErrors.AlreadyExists;
            }

            await context.Roles.AddAsync(
                new Role
                {
                    Name = request.RoleName
                },
                cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}