using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace auth_backend.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            
            _next = next;
            _logger = logger;
        }

        //Forma simple de hacerlo sin implementar la interfaz de IMiddleware
        //public async Task InvokeAsync(HttpContext httpContext)
        //{
        //    try
        //    {
        //        //before the request
        //        await _next(httpContext);
        //        //after the request
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //    }
        //}

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                //before the request
                await next(context);
                //after the request
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                //Creamos objeto para retornar en la response de la request.

                ProblemDetails problem = new() 
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server error",
                    Title = "Server error",
                    Detail = "An internal server has ocurred"
                };

                string json = JsonSerializer.Serialize(problem);
                
                await context.Response.WriteAsync(json);

                context.Response.ContentType = "application/json";
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}
