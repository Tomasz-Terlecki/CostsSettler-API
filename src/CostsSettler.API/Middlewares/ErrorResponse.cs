using System.Net;
using System.Text.Json;

namespace CostsSettler.API.Middlewares;

public class ErrorResponse
{
    public int StatusCode { get; set; } = (int)HttpStatusCode.InternalServerError;
    public string Message { get; set; }

    public ErrorResponse(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
