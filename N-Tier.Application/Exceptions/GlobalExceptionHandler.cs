using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace N_Tier.Application.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, exception.Message);

        ProblemDetails problem;

        switch (exception)
        {
            case ValidationException:
                problem = new ProblemDetails
                {
                    Status = 400,
                    Title = "Validation Error",
                    Detail = exception.Message
                };
                break;

            case UnauthorizedException:
                problem = new ProblemDetails
                {
                    Status = 401,
                    Title = "Unauthorized",
                    Detail = exception.Message
                };
                break;

            case ForbiddenException:
                problem = new ProblemDetails
                {
                    Status = 403,
                    Title = "Forbidden",
                    Detail = exception.Message
                };
                break;

            case NotFoundException:
                problem = new ProblemDetails
                {
                    Status = 404,
                    Title = "Not Found",
                    Detail = exception.Message
                };
                break;

            default:
                problem = new ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred."
                };
                break;
        }

        context.Response.StatusCode = problem.Status!.Value;

        await context.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}