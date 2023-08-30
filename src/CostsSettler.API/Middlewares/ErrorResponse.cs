using System.Net;
using System.Text.Json;

namespace CostsSettler.API.Middlewares;

/// <summary>
/// Representation of error response.
/// </summary>
public class ErrorResponse
{
    private int _statusCode { get; set; } = (int)HttpStatusCode.InternalServerError;
    private string _message { get; set; }

    /// <summary>
    /// Creates new ErrorResponse.
    /// </summary>
    /// <param name="statusCode">HTTP response status code.</param>
    /// <param name="message">Error message sent with a response.</param>
    public ErrorResponse(int statusCode, string message)
    {
        _statusCode = statusCode;
        _message = message;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
