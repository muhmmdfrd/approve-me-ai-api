using System.Net;
using System.Text;
using ApproveMe.Api.Constants;
using ApproveMe.Api.Models;
using Flozacode.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApproveMe.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _request;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate request, ILogger<ExceptionMiddleware> logger)
    {
        _request = request;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext, IWebHostEnvironment environment)
    {
        try
        {
            await _request(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex, environment, _logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext httpContext, Exception ex, IWebHostEnvironment environment, ILogger<ExceptionMiddleware> logger)
    {
        var code = ResponseConstant.INTERNAL_SERVER_ERROR_CODE;
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
            code = ResponseConstant.UNAUTHORIZED_CODE;
            message = unauthorizedException.Message;
            httpStatus = (int)HttpStatusCode.Unauthorized;
        }
        else if (ex is DbUpdateException _)
        {
            code = ResponseConstant.DATABASE_UNIQUE_CODE;
            message = innerMessage;
        }
        else if (ex is InvalidOperationException invalidOperationException)
        {
            if (invalidOperationException.Message.Contains("UseMySql"))
            {
                code = ResponseConstant.DATABASE_CONNECTION_CODE;
                message = ResponseConstant.DATABASE_CONNECTION;
            }
        }
        else if (ex is RecordNotFoundException _)
        {
            code = ResponseConstant.RECORD_NOT_FOUND_CODE;
        }

        var response = new ApiResponse<object>().Fail(message).ToString();

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = httpStatus;

        return httpContext.Response.WriteAsync(response!, Encoding.UTF8);
    }
}