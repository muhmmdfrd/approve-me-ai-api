using Microsoft.IO;

namespace ApproveMe.Api.Middlewares;

public class HttpLoggerMiddleware(RequestDelegate next, ILogger<HttpLoggerMiddleware> logger)
{
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager = new();

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var correlationId = Guid.NewGuid().ToString().ToUpper();
        var notAllowedEndpoint = new[] {"/auth"};
        var notAllowed = notAllowedEndpoint.Any(q => httpContext.Request.Path.Value?.Contains(q) == true);

        await LogRequest(httpContext, correlationId, notAllowed);
        await LogResponse(httpContext, correlationId, notAllowed);   
    }

    private static string ReadStreamInChunks(Stream stream)
    {
        const int readChunkBufferLength = 4096;
        stream.Seek(0, SeekOrigin.Begin);

        using var textWriter = new StringWriter();
        using var reader = new StreamReader(stream);
         
        var readChunk = new char[readChunkBufferLength];
        int readChunkLength;

        do
        {
            readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
            textWriter.Write(readChunk, 0, readChunkLength);
        } while (readChunkLength > 0); 
        
        return textWriter.ToString();    
    }

    private async Task LogRequest(HttpContext context, string correlationId, bool notAllowed = false)
    {
        if (context.Request.Headers.ContainsKey("X-Correlation-ID"))
        {
            context.Request.Headers["X-Correlation-ID"] = correlationId;
        }
        else
        {
            context.Request.Headers.Append("X-Correlation-ID", correlationId);
        }

        context.Request.EnableBuffering();
        
        await using var requestStream = _recyclableMemoryStreamManager.GetStream();
        await context.Request.Body.CopyToAsync(requestStream);

        var method = context.Request.Method;
        var path = context.Request.Path;
        var queryString = context.Request.QueryString;
        var headers = FormatHeaders(context.Request.Headers);
        var scheme = context.Request.Scheme;
        var ipAddress = context.Connection.RemoteIpAddress;
        var body = notAllowed ? string.Empty : ReadStreamInChunks(requestStream);
        
        logger.LogInformation("\n HTTP Request Information:\n " +
                              "Method:{method}\n " +
                              "Path:{path}\n " +
                              "QueryString:{qs}\n " +
                              "Headers:{headers}\n " +
                              "Schema:{scheme}\n " +
                              "RemoteIpAddress:{ip}\n " +
                              "Body:{body}\n", method, path, queryString, headers, scheme, ipAddress, body);

        context.Request.Body.Position = 0;
    }

    private async Task LogResponse(HttpContext context, string correlationId, bool notAllowed = false)
    {
        var originalBodyStream = context.Response.Body;
        await using var responseBody = _recyclableMemoryStreamManager.GetStream();
        context.Response.Body = responseBody;
        await next(context);

        if (context.Response.Headers.ContainsKey("X-Correlation-ID"))
        {
            context.Response.Headers["X-Correlation-ID"] = correlationId;
        }
        else
        {
            context.Response.Headers.Append("X-Correlation-ID", correlationId);
        }

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        logger.LogInformation($"\nHttp Response Information:\n" +
                              $"StatusCode:{context.Response.StatusCode}\n" +
                              $"Request Path:{context.Request.Path}\n" +
                              $"ContentType:{context.Response.ContentType}\n" +
                              $"Headers:{FormatHeaders(context.Response.Headers)}\n" +
                              (notAllowed ? "" : $"Body:{text}\n"));

        await responseBody.CopyToAsync(originalBodyStream);
    }

    private static string FormatHeaders(IHeaderDictionary headers) => string.Join(", ", headers.Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value!)}}}"));
}