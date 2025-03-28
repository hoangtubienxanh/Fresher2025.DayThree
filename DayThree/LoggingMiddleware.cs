using Microsoft.AspNetCore.HttpLogging;

namespace DayThree;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger, TimeProvider timeProvider)
{
    public Task Invoke(HttpContext context)
    {
        var loggingAttribute = context.GetEndpoint()?.Metadata.GetMetadata<HttpLoggingAttribute>();
        var loggingFields = loggingAttribute?.LoggingFields;

        var request = context.Request;

        logger.LogInformation("""
                              [CustomLog]
                              Schema: {schema},
                              Host: {host},
                              Path: {path}
                              Query: {query},
                              Body: {body}
                              """, request.Scheme, request.Host, request.Path, request.QueryString, request.Body);

        return next(context);
    }
}