using System.ComponentModel.DataAnnotations;
using contact.Utility.Language;

namespace contact.Attributes;

public class LanguageValidatorAttribute : ValidationAttribute
{
  public override bool IsValid(object value)
  {
    return CultureHelper.Supported.Contains(value?.ToString().ToLower());
  }
}
