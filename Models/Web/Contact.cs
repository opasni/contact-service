using services.Utility;
using System.ComponentModel.DataAnnotations;

namespace services.Models;

public class Contact : ContactData
{
  public Guid ContactId { get; set; }
}

public class ContactData
{
  [Required, EmailAddress]
  public string Email { get; set; }
  public string Name { get; set; }
  public string Subject { get; set; }
  public string Text { get; set; }
  public string LanguageId { get; set; } = CultureHelper.Default;
}