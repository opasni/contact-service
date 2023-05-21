using System.ComponentModel.DataAnnotations;

namespace contact.Models.Web;

public class ReCaptchaValidation
{
  [Required]
  public Guid ContactId { get; set; }
  [Required]
  public string Recaptcha { get; set; }
}