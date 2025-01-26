using System.Net;
using System.Text;
using ApproveMe.Api.Constants;
using ApproveMe.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ApproveMe.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate request, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext, IWebHostEnvironment environment)
    {
        try
        {
            await request(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex, environment, logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext httpContext, Exception ex, IWebHostEnvironment environment, ILogger<ExceptionMiddleware> logger)
    {
        var message = ResponseConstant.INTERNAL_SERVER_ERROR;
        var innerMessage = ex.InnerException != null ? ex.GetBaseException().Message : string.Empty;
        var exceptionType = ex.GetType().ToString();

        if (environment.IsDevelopment() || environment.IsStaging())
        {
            message = $"{ex.Message} {innerMessage} ({exceptionType})";
        }

        if (environment.IsProduction())
        {
            logger.Log(LogLevel.Error, ex.Message, innerMessage);
        }

        var httpStatus = (int)HttpStatusCode.InternalServerError;

        // check typeof Exception
        if (ex is UnauthorizedAccessException unauthorizedException)
        {
            message = unauthorizedException.Message;
            httpStatus = (int)HttpStatusCode.Unauthorized;
        }
        else if (ex is DbUpdateException _)
        {
            message = innerMessage;
        }
        else if (ex is InvalidOperationException invalidOperationException)
        {
            if (invalidOperationException.Message.Contains("UseMySql"))
            {
                message = ResponseConstant.DATABASE_CONNECTION;
            }
        }

        var response = new ApiResponse<object>().Fail(message).ToString();

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = httpStatus;

        return httpContext.Response.WriteAsync(response, Encoding.UTF8);
    }
}