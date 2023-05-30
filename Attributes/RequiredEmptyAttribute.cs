using System.ComponentModel.DataAnnotations;

namespace contact.Attributes;

public class RequiredEmptyAttribute : ValidationAttribute
{
  public override bool IsValid(object value)
  {
    return string.IsNullOrEmpty(value?.ToString());
  }
}
