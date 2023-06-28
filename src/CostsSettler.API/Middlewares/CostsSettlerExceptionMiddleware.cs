using CostsSettler.Domain.Exceptions;
using System.Net;

namespace CostsSettler.API.Middlewares;

public class CostsSettlerExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public CostsSettlerExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

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
            ObjectReferenceException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
}
