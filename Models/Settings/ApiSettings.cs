namespace services.Models.Settings;

public class ApiSettings
{
  /// <summary>
  /// The API address where to send the ReCaptcha validation requests.
  /// </summary>
  public string GoogleReCaptchaUrl { get; set; }

  /// <summary>
  /// The Secret for the ReCaptcha validation requests.
  /// </summary>
  public string GoogleReCaptchaSecret { get; set; }
}