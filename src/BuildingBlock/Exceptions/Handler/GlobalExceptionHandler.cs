using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace BuildingBlock.Exceptions.Handler
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger): IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var traceId = httpContext.TraceIdentifier;

            logger.LogError("Error Message: {exceptionMessage}, Time of occurrence: {time}, TraceID: {traceId}",
                exception, DateTime.UtcNow, traceId);

            string defaultErrorMessage = "伺服器錯誤";
            (string Detail, string Title, int StatusCode) = exception switch
            {
                InternalServerException =>
                (
                    defaultErrorMessage,
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
                ValidationException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                BadRequestException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                NotFoundException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound
                ),
                _ =>
                (
                    defaultErrorMessage,
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
            };

            var problemDetails = new ProblemDetails()
            {
                Title = Title,
                Detail = Detail,
                Status = StatusCode,
                Instance = httpContext.Request.Path,
            };

            problemDetails.Extensions.Add("TraceID", traceId);

            if (exception is ValidationException validationException)
            {
                problemDetails.Extensions.Add("ValidationError", validationException.Errors);
            }

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
