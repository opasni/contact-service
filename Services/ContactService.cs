using Microsoft.Extensions.Caching.Memory;
using services.Models;
using services.Models.Web;
using services.Services.Api;

namespace services.Services;

public interface IContactService
{
  /// <summary>
  /// Creates a new request for contact in the memory cache until the request is verified by recaptcha.
  /// </summary>
  /// <param name="contactData"></param>
  /// <returns></returns>
  public Contact CreateContactRequest(ContactData contactData);

  /// <summary>
  /// Verifies if the request is present in memory, if the recaptcha is valid and if, sends out the email contact.
  /// </summary>
  /// <param name="validation"></param>
  /// <returns></returns>
  public Task VerifyAndSend(ReCaptchaValidation validation);
}

public class ContactService : IContactService
{
  private readonly ILogger logger;
  private readonly IMemoryCache memoryCache;
  private readonly IEmailService emailService;
  private readonly IReCaptchaApiService reCaptchaApi;

  public ContactService(
     ILogger<ContactService> logger,
     IMemoryCache memoryCache,
     IEmailService emailService,
     IReCaptchaApiService reCaptchaApi)
  {
    this.logger = logger;
    this.memoryCache = memoryCache;
    this.emailService = emailService;
    this.reCaptchaApi = reCaptchaApi;
  }

  public Contact CreateContactRequest(ContactData contactData)
  {
    Guid contactId = Guid.NewGuid();
    memoryCache.Set(contactId, contactData);

    return new Contact
    {
      ContactId = contactId,
      Email = contactData.Email
    };
  }

  public async Task VerifyAndSend(ReCaptchaValidation validation)
  {
    if (!memoryCache.TryGetValue(validation.ContactId, out ContactData contactData))
    {
      throw new KeyNotFoundException();
    }
    if (!await reCaptchaApi.Verify(validation.Recaptcha))
    {
      throw new UnauthorizedAccessException();
    }
    var email = emailService.CreateMail(contactData, contactData.Subject, contactData.Text);

    await emailService.SendAsync(email);
  }
}