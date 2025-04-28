using System.Net;
using System.Text.Json;

namespace Api.Middleware
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
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
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is FluentValidation.ValidationException validationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var errors = validationException.Errors
                    .Select(error => new
                    {
                        field = error.PropertyName,
                        message = error.ErrorMessage
                    });

                var response = new
                {
                    statusCode = context.Response.StatusCode,
                    message = "Validaciones fallidas",
                    errors
                };

                return context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var genericResponse = new
            {
                statusCode = context.Response.StatusCode,
                message = "Ocurrió un error interno."
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(genericResponse));
        }
    }

}
