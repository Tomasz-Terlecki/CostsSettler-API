using CostsSettler.Domain.Exceptions;
using System.Net;

namespace CostsSettler.API.Middlewares;

/// <summary>
/// Middleware that catches exceptions thrown by application.
/// </summary>
public class CostsSettlerExceptionMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Creates new CostsSettlerExceptionMiddleware instance.
    /// </summary>
    /// <param name="next">A function that processes the HTTP requests.</param>
    public CostsSettlerExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes the request with try-catch block, catches all exceptions and maps them into suitable HTTP response codes.
    /// </summary>
    /// <param name="context">An HTTP context to process.</param>
    /// <returns>The task that process the HTTP request given in 'context' parameter.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex) 
        {
            context.Response.StatusCode = (int)GetHttpStatusCode(ex);
            context.Response.ContentType = "application/json";

            var message = ex.Message + Environment.NewLine + ex.StackTrace;

            await context.Response.WriteAsync(new ErrorResponse(context.Response.StatusCode, message).ToString());
        }
    }

    private HttpStatusCode GetHttpStatusCode(Exception ex)
        => ex switch
        {
            ObjectReferenceException or DomainLogicException => HttpStatusCode.BadRequest,
            ObjectNotFoundException => HttpStatusCode.NotFound,
            AuthorizationException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };
}
