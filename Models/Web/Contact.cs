using contact.Attributes;
using contact.Utility.Language;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace contact.Models.Web;

public class Contact : ContactData
{
  public Guid ContactId { get; set; }
}

public class ContactData
{
  /// <summary>
  /// The name of the sender.
  /// </summary>
  [Required(AllowEmptyStrings=false)]
  public string Name { get; set; }

  /// <summary>
  /// The email address to whom reply to.
  /// </summary>
  [JsonPropertyName("replyto")]
  [Required, EmailAddress]
  public string Email { get; set; }

  /// <summary>
  /// The purpose of contact.
  /// </summary>
  [Required(AllowEmptyStrings=false)]
  public string Subject { get; set; }

  /// <summary>
  /// The message.
  /// </summary>
  [Required(AllowEmptyStrings=false)]
  public string Message { get; set; }

  /// <summary>
  /// The message.
  /// </summary>
  [JsonPropertyName("_honeypot")]
  [RequiredEmpty]
  public string HoneyPot { get; set; }

  /// <summary>
  /// The language of the person contacting.
  /// </summary>
  [LanguageValidator]
  public string LanguageId { get; set; } = CultureHelper.Default;
}