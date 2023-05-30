using System.ComponentModel.DataAnnotations;

namespace contact.Models.Web;

public class ReCaptchaValidation
{
  /// <summary>
  /// The identifier of the sent contact request.
  /// </summary>
  [Required]
  public Guid ContactId { get; set; }

  /// <summary>
  /// Recaptcha string to verify.
  /// </summary>
  [Required]
  public string Recaptcha { get; set; }
}