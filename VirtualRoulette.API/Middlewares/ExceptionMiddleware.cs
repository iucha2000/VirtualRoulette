using System.Net;
using System.Text;
using VirtualRoulette.Application.DTOs;
using VirtualRoulette.Domain.Exceptions;

namespace VirtualRoulette.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //Log detailed exception
                await LogExceptionDetailsAsync(context, ex);
                //Format and handle the exception message body
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
                case ValidationException 
                or NotEnoughBalanceException:
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
            var errorResponse = new ExceptionDto
            {
                Status = response.StatusCode,
                Code = exception.GetType().Name,
                Message = errorMessage
            };

            var wrapperResponse = ResponseWrapperDto<ExceptionDto>.Failure(errorResponse, response.StatusCode);
            return response.WriteAsJsonAsync(wrapperResponse);
        }

        private Task LogExceptionDetailsAsync(HttpContext context, Exception exception)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Exception details:");
            sb.AppendLine($"Request Path: {context.Request.Path}");
            sb.AppendLine($"Request Method: {context.Request.Method}");
            sb.AppendLine($"Client IP: {context.Connection.RemoteIpAddress}");
            sb.AppendLine($"User: {context.User?.Identity?.Name ?? "Not authenticated"}");
            sb.AppendLine($"Timestamp: {DateTime.UtcNow}");
            sb.AppendLine($"Exception Type: {exception.GetType().FullName}");
            sb.AppendLine($"Exception Message: {exception.Message}");

            if (exception.Data.Count > 0)
            {
                sb.AppendLine("Exception Data:");
                foreach (var key in exception.Data.Keys)
                {
                    sb.AppendLine($"  {key}: {exception.Data[key]}");
                }
            }

            if (exception.InnerException != null)
            {
                sb.AppendLine("Inner Exceptions:");
                var innerEx = exception.InnerException;
                int level = 1;

                while (innerEx != null)
                {
                    sb.AppendLine($"Level {level} - {innerEx.GetType().FullName}: {innerEx.Message}");
                    innerEx = innerEx.InnerException;
                    level++;
                }
            }

            sb.AppendLine("Stack Trace:");
            sb.AppendLine(exception.StackTrace);

            _logger.LogError(sb.ToString());

            return Task.CompletedTask;
        }
    }
}
