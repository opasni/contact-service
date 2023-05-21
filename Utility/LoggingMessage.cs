namespace contact.Utility
{
  using Microsoft.Extensions.Logging;

  public static class LoggingMessage
  {
    private const string Critical = "CRITICAL occurred | {message}";
    private const string Debug = "DEBUG | {message}";
    private const string Information = "INFO | {message}";
    private const string Error = "ERROR occurred | {message}";
    private const string Warning = "WARNING occurred | {message}";

    public static string GetMessage(LogLevel level, string message = null) => level switch
    {
      LogLevel.Debug => Debug,
      LogLevel.Information => Information,
      LogLevel.Warning => Warning,
      LogLevel.Error => Error,
      LogLevel.Critical => Critical,
      _ => "{message}"
    };

    public const string Unauthorized = "Unauthorized Request on {0}";
    public const string DefaultLanguage = "Will try to fetch the resource for the default language {0}";

    public const string EmailSent = "Email sent to {mail}";
    public const string EmailError = "failed to send email to {email} | {message}";
  }
}