using N_Tier.Application.Common.Abstraction;

namespace N_Tier.Application.Helper.Services.Interfaces;

public interface IEmailService
{
    Task<Result> EmailBody(string to, string subject, string body, CancellationToken cancellation = default);
}