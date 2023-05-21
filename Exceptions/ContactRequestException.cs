using services.Enums;

namespace services.Exceptions;

/// <summary>
/// Simple exception implementation for api responses.
/// </summary>
public class ContactRequestException : Exception
{
  public ErrorType ErrorType { get; set; }
  public int StatusCode { get; } = 400;

  public ContactRequestException(ErrorType errorType, int statusCode = 400) : base()
  {
    ErrorType = errorType;
    StatusCode = statusCode;
  }
}