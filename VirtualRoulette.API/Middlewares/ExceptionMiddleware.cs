using System.Net;
using VirtualRoulette.Domain.Exceptions;

namespace VirtualRoulette.API.Middlewares
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            int statusCode;
            string errorMessage;

            switch (exception)
            {
                case ValidationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errorMessage = exception.Message;
                    break;
                case EntityNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    errorMessage = exception.Message;
                    break;
                case DuplicateEntityException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    errorMessage = exception.Message;
                    break;
                case DatabaseException:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    errorMessage = "A database error occurred: " + exception.Message;
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    errorMessage = "An unexpected error occurred.";
                    break;
            }

            response.StatusCode = statusCode;
            var errorResponse = new
            {
                Status = response.StatusCode,
                Code = exception.GetType().Name,
                Message = errorMessage
            };

            return response.WriteAsJsonAsync(errorResponse);
        }
    }
}
