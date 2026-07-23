using Microsoft.AspNetCore.Diagnostics;
using N_Tier.Application.Common.Exceptions;

namespace N_Tier.API.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, exception.Message);

        var problem = exception switch
        {
            UnauthorizedException => new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = exception.Message
            },

            ForbiddenException => new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
                Detail = exception.Message
            },

            NotFoundException => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = exception.Message
            },

            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred."
            }
        };

        context.Response.StatusCode = problem.Status!.Value;

        await context.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}