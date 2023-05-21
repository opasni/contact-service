using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using contact.Enums;
using contact.Exceptions;
using contact.Models.Web;
using contact.Utility;

namespace contact.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
  private ILogger logger;

  public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger) : base()
  {
    this.logger = logger;
  }

  public override void OnException(ExceptionContext context)
  {
    ErrorMessage error;
    if (context.Exception is UnauthorizedAccessException)
    {
      error = new ErrorMessage(ErrorType.UnauthorizedAccess);
      context.HttpContext.Response.StatusCode = 401;
    }
    else if (context.Exception is NotImplementedException || context.Exception is NotSupportedException)
    {
      error = new ErrorMessage(ErrorType.NotImplemented);
      context.HttpContext.Response.StatusCode = 501;
    }
    else if (context.Exception is ContactRequestException)
    {
      var exception = context.Exception as ContactRequestException;
      error = new ErrorMessage(exception.ErrorType);
      context.HttpContext.Response.StatusCode = exception.StatusCode;
    }
    else if (context.Exception is KeyNotFoundException)
    {
      error = new ErrorMessage(ErrorType.InvalidId);
      context.HttpContext.Response.StatusCode = 404;
    }
    else
    {
      logger.LogError(context.Exception, LoggingMessage.GetMessage(LogLevel.Error), context.Exception.Message);
      error = new ErrorMessage(ErrorType.Unhandled);
      context.HttpContext.Response.StatusCode = 500;
    }

#if DEBUG
    var message = CreateDebugMessage(context.Exception);
    logger.LogDebug(context.Exception, LoggingMessage.GetMessage(LogLevel.Debug), message);
    error.Message = message;
#endif

    // always return a JSON result
    context.Result = new JsonResult(error);

    base.OnException(context);
  }

  private static string CreateDebugMessage(Exception exception) => $"{exception.GetBaseException().Message} | Stack Trace: {exception.StackTrace} | Inner Exception: {exception.InnerException}";
}