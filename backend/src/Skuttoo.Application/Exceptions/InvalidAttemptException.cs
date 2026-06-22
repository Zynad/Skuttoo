namespace Skuttoo.Application.Exceptions;

/// <summary>
/// Thrown when an attempt payload does not match the exercise type (e.g. a drag-to-bucket
/// exercise received no placements). Mapped to a 400 ProblemDetails by the API middleware.
/// </summary>
public sealed class InvalidAttemptException : Exception
{
    public InvalidAttemptException(string message) : base(message)
    {
    }
}
