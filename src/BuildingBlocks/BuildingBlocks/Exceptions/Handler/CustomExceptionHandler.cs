using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
      logger.LogError("Error message: {message}, Time of occurrance: {time}", 
          exception.Message, DateTime.UtcNow);

        (string Detail, string Title, int StatusCode) details = exception switch
        {
            NotFoundException =>
            (
            exception.Message,
            exception.GetType().Name,
            context.Response.StatusCode = StatusCodes.Status404NotFound
            ),
            BadRequestException =>
            (
            exception.Message,
            exception.GetType().Name,
            StatusCodes.Status400BadRequest
            ),
            ValidationException =>
            (
              exception.Message,
              exception.GetType().Name,
              context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            BadHttpRequestException =>
            (
              exception.Message,
              exception.GetType().Name,
              context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            InternalServerException =>
            (
            exception.Message,
            exception.GetType().Name,
            StatusCodes.Status500InternalServerError
            ),
            _ => (
            exception.Message,
            exception.GetType().Name,
            StatusCodes.Status500InternalServerError
            )
        };

        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Detail,
            Status = details.StatusCode,
            Instance = context.Request.Path
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        if (exception is ValidationException validationException)
        {
            //problemDetails.Extensions.Add("ValidationErrors", exception.);
           
        }

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
