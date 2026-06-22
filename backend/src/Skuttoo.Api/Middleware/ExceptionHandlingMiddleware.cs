using Skuttoo.Application.Exceptions;

namespace Skuttoo.Api.Middleware;

/// <summary>
/// Maps domain exceptions to RFC 7807 ProblemDetails:
/// <see cref="NotFoundException"/> -> 404, anything unhandled -> 500.
/// Never leaks stack traces to clients.
/// </summary>
public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, IProblemDetailsService problemDetails)
    {
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation(ex, "Resource not found: {Message}", ex.Message);
            await WriteProblemAsync(context, problemDetails, StatusCodes.Status404NotFound, "Not Found", ex.Message)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception processing {Path}", context.Request.Path);
            await WriteProblemAsync(
                context,
                problemDetails,
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred.")
                .ConfigureAwait(false);
        }
    }

    private static async Task WriteProblemAsync(
        HttpContext context,
        IProblemDetailsService problemDetails,
        int statusCode,
        string title,
        string detail)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Clear();
        context.Response.StatusCode = statusCode;

        await problemDetails.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            ProblemDetails =
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
            },
        }).ConfigureAwait(false);
    }
}
