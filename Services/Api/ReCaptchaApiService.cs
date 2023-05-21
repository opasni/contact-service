using Microsoft.Extensions.Options;
using contact.Exceptions;
using contact.Models.Api.ReCaptcha;
using contact.Models.Settings;

namespace contact.Services.Api
{
  public interface IReCaptchaApiService
  {
    /// <summary>
    /// Verify the validity of the provided ReCaptcha request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<bool> Verify(string request);
  }

  /// <summary>
  /// implementation of ReCaptchaApiService.
  /// </summary>
  public class ReCaptchaApiService : ApiServiceBase, IReCaptchaApiService
  {

    public ReCaptchaApiService(
      ILogger<ReCaptchaApiService> logger,
      IOptions<ApiSettings> options)
      : base(logger, options.Value)
    { }

    public async Task<bool> Verify(string request)
    {
      var form = new List<KeyValuePair<string, string>>
      {
          new KeyValuePair<string, string>("secret", ApiSettings.GoogleReCaptchaSecret),
          new KeyValuePair<string, string>("response", request)
      };
      var url = ApiSettings.GoogleReCaptchaUrl;

      var verification = await PostAsync<VerificationResponse>(url, form);
      if (!verification.IsSuccess)
      {
        throw new ContactRequestException(Enums.ErrorType.InvalidPayload);
      }
      return true;
    }
  }
}
