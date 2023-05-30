using contact.Enums;
using contact.Utility;

namespace contact.Models.Web;
public class ErrorMessage
{
  /// <summary>
  /// Gets or sets the message.
  /// </summary>
  /// <value>The message.</value>
  public string Message { get; set; }

  public ErrorMessage(ErrorType error) => Message = error.GetDescription();
}