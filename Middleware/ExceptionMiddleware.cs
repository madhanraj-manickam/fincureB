using System.Net;
using System.Text.Json;
using FinCure.Models;

namespace FinCure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) {
                await HandleExceptionAsync(context, ex);
            }

        }

        // this version will only return internal service error for all exceptions(500)
        //public Task HandleExceptionAsync(HttpContext context , Exception exception)
        //{
        //    context.Response.ContentType = "application/json";
        //    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //    var errorResponse = new Models.ErrorResponse
        //    {
        //        StatusCode = context.Response.StatusCode,
        //        Message = exception.Message,
        //        TimeStamp = DateTime.UtcNow
        //    };
        //    return context.Response.WriteAsJsonAsync(errorResponse);
        //}

        private static async Task HandleExceptionAsync(
           HttpContext context,
           Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                TimeStamp = DateTime.UtcNow
            };

            switch (exception)
            {
                case ArgumentException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = exception.Message;
                    break;

                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = exception.Message;
                    break;

                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = exception.Message;
                    break;

                default:
                    response.StatusCode =
                        (int)HttpStatusCode.InternalServerError;

                    response.Message =
                        "An unexpected error occurred." + exception.Message;
                    break;
            }

            context.Response.StatusCode = response.StatusCode;

            var json =
                JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }



    }
}
