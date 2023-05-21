using contact.Enums;

namespace contact.Models.Web;
public class ErrorMessage
{
  /// <summary>
  /// Gets or sets the message.
  /// </summary>
  /// <value>The message.</value>
  public string Message { get; set; }

  /// <summary>
  /// Gets or sets the message.
  /// </summary>
  /// <value>The message.</value>
  public string Problem { get; set; }

  public ErrorMessage(ErrorType error, string problem = null)
  {

  }
}