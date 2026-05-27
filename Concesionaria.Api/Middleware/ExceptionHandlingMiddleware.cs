using System.Net;
using FluentValidation; 
using Microsoft.AspNetCore.Http;

namespace TuProyecto.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex) 
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors
                    .Select(e => new 
                    { 
                        PropertyName = e.PropertyName, 
                        ErrorMessage = e.ErrorMessage 
                    });

                await context.Response.WriteAsJsonAsync(new { Errors = errors });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { Message = ex.Message, Detail = ex.InnerException?.Message });
            }
        }
    }
}