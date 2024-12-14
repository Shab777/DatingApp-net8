using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //check if it is in developement mode
            var response = env.IsDevelopment()
                ? new ApiExeception(context.Response.StatusCode, ex.Message, ex.StackTrace)
                // if it not
                : new ApiExeception(context.Response.StatusCode, ex.Message, "Internal server error");

            // return in json hence it sould be camel case
            var option = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, option);

            await context.Response.WriteAsync(json);
        }

    }
}
