using ApproveMe.Api.Middlewares;
using Microsoft.Extensions.FileProviders;

namespace ApproveMe.Api.Extensions;

public static class AppBuilderExtension
{
    public static void RegisterMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<HttpLoggerMiddleware>(); // Place HttpLoggerMiddleware first!
        app.UseMiddleware<ExceptionMiddleware>();
    }

    public static void RegisterSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    public static void RegisterCors(this WebApplication app)
    {
        app.UseCors(options =>
        {
            options.SetIsOriginAllowed(_ => true);
            options.AllowAnyHeader();
            options.AllowAnyMethod();
            options.AllowCredentials();
        });
    }
}